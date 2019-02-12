using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Finkit.ManicTime.Client.Main.Logic;
using Finkit.ManicTime.Common;
using Finkit.ManicTime.Plugins.Timelines.Tags;
using Finkit.ManicTime.Shared.Tags.Labels;
using TimeTrackingService;
using TimeTrackingService.Internal;

namespace TagPlugin.ExportTags
{
    public class TagsExporter
    {
        private readonly TimeTrackingClient _client;

        private readonly string _billableActivityId;

        private readonly string _nonBillableActivityId;

        public TagsExporter(string timeTrackerToken, string billableActivityId, string nonBillableActivityId)
        {
            _client = new TimeTrackingClient(timeTrackerToken);
            _billableActivityId = billableActivityId;
            _nonBillableActivityId = nonBillableActivityId;
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

            foreach (var tagActivity in filteredTags)
            {
                var request = ConstructCreateWorkLogRequest(tagActivity, me.User.Id);

                var existingFormat = new Regex($@"via{ClientPlugin.HiddenTagLabel}\[(\d+)\]");

                var existingLog = filteredWorkItems
                    .SingleOrDefault(item =>
                    {
                        Match match = existingFormat.Match(item.Comment);

                        if (match.Success && int.Parse(match.Groups[1].Value) == tagActivity.ActivityId)
                        {
                            return true;
                        }

                        return false;
                    });

                if (existingLog != null)
                {
                    filteredWorkItems = filteredWorkItems
                            .Where(item => item != existingLog);

                    if (existingLog.Timestamp == request.TimeStamp &&
                        existingLog.Length == request.Length &&
                        existingLog.WorkItemId == request.WorkItemId &&
                        existingLog.ActivityType.Id == request.ActivityTypeId &&
                        existingLog.Comment == request.Comment)
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

            var activityTypeId = billable ? _billableActivityId : _nonBillableActivityId;

            return new CreateWorkLogRequest(
                timeStamp: tagActivity.StartTime.UtcDateTime,
                length: Convert.ToInt32(tagActivity.Duration.TotalSeconds),
                workItemId: workItemId,
                comment: $"via{ClientPlugin.HiddenTagLabel}[{tagActivity.ActivityId}]\n{tagActivity.Notes}",
                userId: userId,
                activtyTypeId: activityTypeId);
        }

        public static DateRange GetDateRange()
        {
            var from = DateTime.Today.AddDays(-7);

            var to = DateTime.Today;

            return new DateRange(DateTimeHelper.FromUnshiftedDateTime(from), DateTimeHelper.FromUnshiftedDateTime(to));
        }
    }
}