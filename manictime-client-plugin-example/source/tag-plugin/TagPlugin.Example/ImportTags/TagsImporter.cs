using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finkit.ManicTime.Common.TagSources;
using Finkit.ManicTime.Shared.Tags.Labels;
using TimeTrackingService;
using TimeTrackingService.Internal;
using WorkItemServices;
using WorkItemServices.Internal;

namespace TagPlugin.ImportTags
{
    public class TagsImporter
    {
        private readonly WorkItemClient _workItemClient;

        private readonly TimeTrackingClient _timeTrackingClient;

        private readonly string _billableQueryTemplate;

        private readonly string _nonBillableQueryTemplate;

        public TagsImporter(TagsImporterConfig config)
        {
            _workItemClient = new WorkItemClient(config.PersonalAccessToken, config.Organization);
            _timeTrackingClient = new TimeTrackingClient(config.TimeTrackingToken, config.Organization);
            _billableQueryTemplate = config.BillableQueryTemplate;
            _nonBillableQueryTemplate = config.NonBillableQueryTemplate;
        }

        public async Task<List<TagSourceItem>> GetTagsAsync()
        {
            Me me = await _timeTrackingClient.GetMe();

            IEnumerable<TagSourceItem> billableTags = await GetBillableTagsAsync(me);

            IEnumerable<TagSourceItem> nonBillableTags = await GetNonBillableTagsAsync(me);

            return billableTags
                .Concat(nonBillableTags)
                .ToList();
        }

        private async Task<IEnumerable<TagSourceItem>> GetBillableTagsAsync(Me me)
        {
            IEnumerable<WorkItem> billableWorkItems = await GetWorkItems(me, _billableQueryTemplate);

            return billableWorkItems
                .Select(AsTagSourceItem(billable: true));
        }

        private async Task<IEnumerable<TagSourceItem>> GetNonBillableTagsAsync(Me me)
        {
            IEnumerable<WorkItem> nonBillableWorkItems = await GetWorkItems(me, _nonBillableQueryTemplate);

            return nonBillableWorkItems
                .Select(AsTagSourceItem(billable: false));
        }

        private async Task<IEnumerable<WorkItem>> GetWorkItems(Me me, string queryTemplate)
        {
            WiqlResponse workItemRefsResponse = await _workItemClient
                .GetAssignedWorkItemReferences(me.User.UniqueName, queryTemplate);

            return await _workItemClient
                .GetWorkItemsByReference(workItemRefsResponse.WorkItems);
        }

        private static Func<WorkItem, TagSourceItem> AsTagSourceItem(bool billable) => (WorkItem workItem) =>
        {
            List<string> tags = new List<string> { $"#{workItem.Id}", workItem.Fields.TeamProject, ClientPlugin.HiddenTagLabel };

            if (billable)
            {
                tags.Add(TagLabels.Billable);
            }

            return new TagSourceItem
            {
                Tags = tags.ToArray(),
                Notes = workItem.Fields.Title
            };
        };
    }
}