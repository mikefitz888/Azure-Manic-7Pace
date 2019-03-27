using System;
using System.Collections.Generic;
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

        private readonly DateTime _start = DateTime.Today.AddDays(-7);

        private readonly DateTime _end = DateTime.Today.AddDays(1);

        public TagsExporter(string organizationName, string timeTrackerToken, string billableActivityId, string nonBillableActivityId)
        {
            var baseUrl = $"https://{organizationName}.timehub.7pace.com/api/rest/";

            _client = new TimeTrackingClient(timeTrackerToken, baseUrl);
            _billableActivityId = billableActivityId;
            _nonBillableActivityId = nonBillableActivityId;
        }

        public async Task Export(TagActivity[] unfilteredTagActivities, DateRange range)
        {
            var tags = GetTags(unfilteredTagActivities);

            var workLogs = GetWorkLogs()
                .ToList();

            var me = await _client.GetMe();

            foreach (var tagActivity in tags)
            {
                var createWorkLogRequest = ConstructCreateWorkLogRequest(tagActivity, me.User.Id);

                var existingDuplicateWorkLogs = FindDuplicateExistingWorkLogs(workLogs, tagActivity)
                    .ToList();

                var existingConflictWorkLog = await FirstOrDefaultDeleteRemaining(existingDuplicateWorkLogs);

                // Remove handled WorkLogs from list
                workLogs = workLogs
                    .Where(log => existingDuplicateWorkLogs.Contains(log) == false)
                    .ToList();

                await CreateIdempotent(existingConflictWorkLog, createWorkLogRequest);
            }

            foreach (var item in workLogs)
            {
                await _client.DeleteWorkLog(item.Id);
            }
        }

        public static DateRange GetDateRange()
        {
            var from = DateTime.Today.AddDays(-7);

            var to = DateTime.Today;

            return new DateRange(DateTimeHelper.FromUnshiftedDateTime(from), DateTimeHelper.FromUnshiftedDateTime(to));
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

        private IEnumerable<TagActivity> GetTags(TagActivity[] tagActivities)
        {
            foreach (TagActivity tagActivity in tagActivities)
            {
                if (tagActivity.Groups.Any(tag => tag.Key == ClientPlugin.HiddenTagLabel.ToLower()) == false)
                {
                    continue;
                }

                if (tagActivity.StartTime.DateTime < _start || tagActivity.StartTime.DateTime > _end)
                {
                    continue;
                }

                yield return tagActivity;
            }
        }

        private IEnumerable<WorkLog> GetWorkLogs()
        {
            var workLogs = _client.GetWorkLogs(_start, _end).Result;

            foreach (WorkLog workLog in workLogs)
            {
                if (workLog.Flags.IsFromApi == false)
                {
                    continue;
                }

                yield return workLog;
            }
        }

        private IEnumerable<WorkLog> FindDuplicateExistingWorkLogs(IEnumerable<WorkLog> workLogs, TagActivity tagActivity)
        {
            var existingFormat = new Regex($@"via{ClientPlugin.HiddenTagLabel}\[(\d+)\]");

            foreach (WorkLog workLog in workLogs)
            {
                Match match = existingFormat.Match(workLog.Comment);

                if (match.Success == false || int.Parse(match.Groups[1].Value) != tagActivity.ActivityId)
                {
                    continue;
                }

                yield return workLog;
            }
        }

        private async Task<WorkLog> FirstOrDefaultDeleteRemaining(IList<WorkLog> workLogs)
        {
            var workLogsToDelete = workLogs.Skip(1);

            foreach (WorkLog workLog in workLogsToDelete)
            {
                await _client.DeleteWorkLog(workLog.Id);
            }

            return workLogs.FirstOrDefault();
        }

        private bool IsDuplicate(WorkLog workLog, CreateWorkLogRequest request)
        {
            return workLog.Timestamp == request.TimeStamp &&
                   workLog.Length == request.Length &&
                   workLog.WorkItemId == request.WorkItemId &&
                   workLog.ActivityType.Id == request.ActivityTypeId &&
                   workLog.Comment == request.Comment;
        }

        private async Task CreateIdempotent(WorkLog existingConflictWorkLog, CreateWorkLogRequest createWorkLogRequest)
        {
            if (existingConflictWorkLog != null)
            {
                if (IsDuplicate(existingConflictWorkLog, createWorkLogRequest))
                {
                    return;
                }

                await _client.DeleteWorkLog(existingConflictWorkLog.Id);
            }

            await _client.CreateWorkLog(createWorkLogRequest);
        }
    }
}