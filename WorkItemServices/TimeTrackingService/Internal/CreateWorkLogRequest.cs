using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
            ActivityType activtyType)
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
            ActivityTypeId = GetActivityTypeId(activtyType);
        }

        private static string GetActivityTypeId(ActivityType activityType)
        {
            switch (activityType)
            {
                case ActivityType.Development:
                    return "990971f5-00cb-418f-a3cd-fbff240e0342";
                case ActivityType.Internal:
                    return "0e585326-4d4c-43bb-80fc-835862ff69e4";
                case ActivityType.OutOfOffice:
                    return "88a0c114-9746-4867-95f8-1811782a6bba";
                case ActivityType.Planning:
                    return "f4cbb022-91b6-4c9c-b9e1-591681a06b91";
                case ActivityType.PreSales:
                    return "ba046b19-863f-4999-b737-a5cba04d1083";
                case ActivityType.QA:
                    return "15e719c9-373d-45fa-9b9e-a94dbd4832d4";
                case ActivityType.Support:
                    return "eb892560-196f-4872-9047-d06b9694a765";
                case ActivityType.NotSet:
                default:
                    return "00000000-0000-0000-0000-000000000000";
            }
        }
    }
}
