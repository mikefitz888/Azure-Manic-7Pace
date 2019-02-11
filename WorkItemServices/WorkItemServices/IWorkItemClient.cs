using System.Collections.Generic;
using System.Threading.Tasks;
using WorkItemServices.Internal;

namespace WorkItemServices
{
    public interface IWorkItemClient
    {
        Task<WiqlResponse> GetAssignedWorkItemReferences(string uniqueName, string queryTemplate);
        Task<IEnumerable<WorkItem>> GetWorkItemsByReference(IEnumerable<WorkItemReference> workItemReferences);
    }
}