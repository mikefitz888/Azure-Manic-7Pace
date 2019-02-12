using System;
using System.Threading.Tasks;
using TimeTrackingService;
using TimeTrackingService.Internal;
using WorkItemServices;

namespace WorkItemService.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string personalAccessToken = @"";

            const string timeTrackerToken = @"";

            WorkItems(personalAccessToken).Wait();

            TimeTracker(timeTrackerToken).Wait();
        }

        private static async Task WorkItems(string personalAccessToken)
        {
            var client = new WorkItemClient(personalAccessToken, "<organizationName>");

            var assignedWorkItemReferences = await client.GetAssignedWorkItemReferences("<uniqueUserName>", "query");

            var workItems = await client.GetWorkItemsByReference(assignedWorkItemReferences.WorkItems);
        }

        private static async Task TimeTracker(string timeTrackerToken)
        {
            var client = new TimeTrackingClient(timeTrackerToken);

            var me = await client.GetMe();

            var items = await client.GetWorkLogs(DateTime.Now.AddDays(-7), DateTime.Now);

            var createRequest = new CreateWorkLogRequest(DateTime.UtcNow, 500, null, "TimeTracker API Test", me.User.Id, "00000000-0000-0000-0000-000000000000");

            // var created = await client.CreateWorkLog(createRequest);

            // await client.DeleteWorkLog(created.Id);
        }
    }
}
