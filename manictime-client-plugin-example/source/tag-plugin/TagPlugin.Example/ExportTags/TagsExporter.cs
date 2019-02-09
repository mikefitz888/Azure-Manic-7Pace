using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Finkit.ManicTime.Client.Main.Logic;
using Finkit.ManicTime.Common;
using Finkit.ManicTime.Plugins.Timelines.Tags;
using Finkit.ManicTime.Shared.Logging;
using Finkit.ManicTime.Shared.Tags.Labels;
using TimeTrackingService;
using TimeTrackingService.Internal;
using TimeTrackingService.Mapping;

namespace TagPlugin.ExportTags
{
    public class TagsExporter
    {
        private readonly TimeTrackingClient _client;

        public TagsExporter(string timeTrackerToken)
        {
            _client = new TimeTrackingClient(timeTrackerToken);
        }

        public async Task Export(TagActivity[] tagActivities, DateRange range)
        {
            // Only include tags with hidden display key
            var filteredTags = tagActivities
                .Where(ta => ta.Groups.Any(tag => tag.Key == ClientPlugin.HiddenTagLabel.ToLower()));

            var existingWorkItems = await _client.GetWorkLogs(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(1));

            var filteredWorkItems = existingWorkItems
                .Where(item => item.Flags.IsFromApi == true);

            var me = await _client.GetMe();

            foreach (var tagActivity in tagActivities)
            {
                var request = ConstructCreateWorkLogRequest(tagActivity, me.User.Id);

                var existingLog = filteredWorkItems.SingleOrDefault(item => int.Parse(item.Comment) == tagActivity.ActivityId);

                if (existingLog != null)
                {
                    filteredWorkItems = filteredWorkItems
                            .Where(item => item != existingLog);

                    if (existingLog.Timestamp == request.TimeStamp &&
                        existingLog.Length == request.Length &&
                        existingLog.WorkItemId == request.WorkItemId &&
                        existingLog.ActivityType.Id == request.ActivityTypeId)
                    {
                        continue;
                    }

                    await _client.DeleteWorkLog(existingLog.Id);
                }

                await _client.CreateWorkLog(request);
            }

            foreach (var item in filteredWorkItems)
            {
                await _client.DeleteWorkLog(item.Id);
            }
        }

        private CreateWorkLogRequest ConstructCreateWorkLogRequest(TagActivity tagActivity, string userId)
        {
            var workItemGroup = tagActivity.Groups.Where(g => g.Key.StartsWith("#")).Single();

            var workItemId = workItemGroup.Key.TrimStart('#');

            bool billable = tagActivity.Groups.Any(g => g.Key == TagLabels.Billable);

            var activityType = billable ? ActivityType.Development : ActivityType.Internal;

            return new CreateWorkLogRequest(
                tagActivity.StartTime.UtcDateTime,
                Convert.ToInt32(tagActivity.Duration.TotalSeconds),
                workItemId,
                tagActivity.ActivityId.ToString(),
                userId,
                activityType);
        }

        public static DateRange GetDateRange()
        {
            var from = DateTime.Today.AddDays(-7);

            var to = DateTime.Today;

            return new DateRange(DateTimeHelper.FromUnshiftedDateTime(from), DateTimeHelper.FromUnshiftedDateTime(to));
        }

        /*
            Tag activities for the selected date range. All activities are returned. 
            We usually append a hidden tag (the ones preceded with a colon (:)) so we know which plugin was responsible for them.
            Below we filter the list to get only tags from this plugin. You can easily just skip that and process all of them.
            
            In this sample we only get the tags from this plugin, then save them to a local file.
        */
        public static void ExportTags(TagActivity[] allTagActivities, DateRange range)
        {
            var pluginTags = allTagActivities
                .Where(ta => ta.Groups.Select(g => g.DisplayKey.ToLower()).Contains(ClientPlugin.HiddenTagLabel.ToLower()));

            var rows = pluginTags.Select(t => t.DisplayName + "\t" + t.StartTime + "\t" + t.EndTime);
            var exportString = rows.Aggregate("", (sum, row) => sum + (sum == "" ? "" : "\n") + row);

            if (!Directory.Exists("c:\\ManicTimeData"))
                Directory.CreateDirectory("c:\\ManicTimeData");

            File.WriteAllText("c:\\ManicTimeData\\sampleExport.txt", exportString);
            ApplicationLog.WriteInfo("Data exported to: c:\\ManicTimeData\\sampleExport.txt");
            MessageBox.Show("Data exported to c:\\ManicTimeData\\sampleExport.txt");
        }
    }
}