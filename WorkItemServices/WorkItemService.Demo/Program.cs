using System;
using WorkItemServices;

namespace WorkItemService.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string personalAccessToken = "";

            var client = new WorkItemClient(personalAccessToken);

            var assignedWorkItemReferences = client.GetAssignedWorkItemReferences().Result.WorkItems;

            var workItems = client.GetWorkItemsByReference(assignedWorkItemReferences).Result;

            Console.ReadKey();
        }
    }
}
