using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Finkit.ManicTime.Client.LocalActivityFetching.Messaging;
using Finkit.ManicTime.Client.Main.Logic;
using Finkit.ManicTime.Common;
using Finkit.ManicTime.Common.TagSources;
using Finkit.ManicTime.Common.Timelines;
using Finkit.ManicTime.Plugins.Timelines.Tags;
using Finkit.ManicTime.Shared;
using Finkit.ManicTime.Shared.Logging;
using Finkit.ManicTime.Shared.Plugins.ServiceProviders.PluginCommands;
using TagPlugin.Settings;
using TagPlugins.Core;

namespace TagPlugin.ExportTags
{
    public class ExportTagsCommand : PluginCommand
    {
        private readonly TagSourceService _tagSourceService;
        private readonly ActivityReaderMessageClient _activityReaderMessageClient;
        private readonly IViewTimelineCache _viewTimelineCache;

        // need a mutex on changes to this
        private static bool processing = false;
        private static bool changesQueued = false;

        public ExportTagsCommand(
            IEventHub eventHub, 
            TagSourceService tagSourceService,
            ActivityReaderMessageClient activityReaderMessageClient,
            IViewTimelineCache viewTimelineCache)
        {
            _tagSourceService = tagSourceService;
            _activityReaderMessageClient = activityReaderMessageClient;
            _viewTimelineCache = viewTimelineCache;
            eventHub.Subscribe<TagSourceCacheUpdatedEvent>(OnTagSourceCacheUpdated);
            InvokeOnUiThread(SetCanExecute);
        }

        public override string Name => "Publish Work Logs";

        private static void InvokeOnUiThread(Action action)
        {
            var currentApplication = Application.Current;
            if (currentApplication == null || currentApplication.CheckAccess())
                action();
            else
                currentApplication.Dispatcher.Invoke(action);
        }

        private void OnTagSourceCacheUpdated(TagSourceCacheUpdatedEvent obj)
        {
            InvokeOnUiThread(SetCanExecute);

            if (CanExecute)
            {
                if (processing == false)
                {
                    Execute();
                }
                else
                {
                    changesQueued = true;
                }
            }
        }

        private void SetCanExecute()
        {
            CanExecute =
                TagPluginsHelper.GetTagSourceInstances(_tagSourceService.GetTagSourceInstances(),
                    ClientPlugin.Id).Any();
        }

        public override async void Execute()
        {
            processing = true;

            try
            {
                var tagSourceInstance = TagPluginsHelper.GetTagSourceInstances(
                    _tagSourceService.GetTagSourceInstances(),
                    ClientPlugin.Id)
                    .First();

                var azureDevOpsSettings = (AzureDevOpsWorkItemTagSettings)tagSourceInstance.Settings ?? new AzureDevOpsWorkItemTagSettings();

                var exporter = new TagsExporter(azureDevOpsSettings.TimeTrackerApiSecret, azureDevOpsSettings.BillableActivityId, azureDevOpsSettings.NonBillableActivityId);

                DateRange range = TagsExporter.GetDateRange();

                var tagActivities = await GetTagActivitiesAsync(range.From, range.To).ConfigureAwait(false);

                await exporter.Export(tagActivities, range);
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteError(ex);
            }

            processing = false;

            if (changesQueued)
            {
                changesQueued = false;
                Execute();
            }
        }

        private async Task<TagActivity[]> GetTagActivitiesAsync(int dateFrom, int dateTo)
        {
            var timeline = _viewTimelineCache.LocalTagTimeline;

            var activities = await _activityReaderMessageClient.GetActivitiesAsync(
                timeline,
                new Date(dateFrom.AsStartDateTime()),
                new Date(dateTo.AsStartDateTime()),
                false,
                CancellationToken.None).ConfigureAwait(false);

            return activities.Cast<TagActivity>().ToArray();
        }
    }
}