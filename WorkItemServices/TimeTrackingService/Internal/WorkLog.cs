using System;

namespace TimeTrackingService.Internal
{
    public class WorkLog
    {
        public string Id { get; }

        public DateTime Timestamp { get; }

        public int Length { get; }

        public int BillableLength { get; }

        public string WorkItemId { get; }

        public string Comment { get; }

        public DateTime CreatedTimestamp { get; }

        public DateTime EditedTimestamp { get; }

        public ActivityTypeResponse ActivityType { get; }

        public Flags Flags { get; }

        public WorkLog(
            string id,
            DateTime timestamp,
            int length,
            int? billableLength,
            string workItemId,
            string comment,
            DateTime createdTimestamp,
            DateTime editedTimestamp,
            ActivityTypeResponse activityType,
            Flags flags)
        {
            Id = id;
            Timestamp = timestamp;
            Length = length;
            BillableLength = billableLength ?? 0;
            WorkItemId = workItemId;
            Comment = comment;
            CreatedTimestamp = createdTimestamp;
            EditedTimestamp = editedTimestamp;
            ActivityType = activityType;
            Flags = flags;
        }
    }
}
