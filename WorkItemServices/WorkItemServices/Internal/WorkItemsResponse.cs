using System.Collections.Generic;

namespace WorkItemServices.Internal
{
    public class WorkItemsResponse
    {
        public IEnumerable<WorkItem> Value { get; }

        public WorkItemsResponse(IEnumerable<WorkItem> value)
        {
            Value = value;
        }
    }
}
