using System;

namespace TimeTrackingService.Internal
{
    public class CreateWorkLogRequest
    {
        public DateTime TimeStamp { get; }

        public int Length { get; }

        public int BillableLength { get; }

        public string WorkItemId { get; }

        public string Comment { get; }

        public string UserId { get; }

        public string ActivityTypeId { get; }

        public CreateWorkLogRequest(
            DateTime timeStamp,
            int length,
            string workItemId,
            string comment,
            string userId,
            string activtyTypeId)
        {
            if (string.IsNullOrWhiteSpace(workItemId) && string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException($"At least one of {nameof(workItemId)} and {nameof(comment)} must not be null or white space.");
            }

            TimeStamp = timeStamp;
            Length = length;
            BillableLength = 0;
            WorkItemId = workItemId;
            Comment = comment;
            UserId = userId;
            ActivityTypeId = activtyTypeId;
        }
    }
}
