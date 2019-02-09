using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finkit.ManicTime.Common.TagSources;
using Finkit.ManicTime.Shared.Tags.Labels;
using WorkItemServices;

namespace TagPlugin.ImportTags
{
    public class TagsImporter
    {
        private readonly WorkItemClient _workItemClient;

        public TagsImporter(string organization, string personalAccessToken)
        {
            _workItemClient = new WorkItemClient(personalAccessToken);
        }

        public async Task<List<TagSourceItem>> GetTags()
        {
            var workItemRefsResponse = await _workItemClient
                .GetAssignedWorkItemReferences();

            var workItems = await _workItemClient
                .GetWorkItemsByReference(workItemRefsResponse.WorkItems);

            var tags = workItems
                .Select(item => new TagSourceItem
                {
                    Tags = new[] { $"#{item.Id}", item.Fields.TeamProject, TagLabels.Billable, ClientPlugin.HiddenTagLabel },
                    Notes = item.Fields.Title
                });

            return tags.ToList();
        }
    }
}