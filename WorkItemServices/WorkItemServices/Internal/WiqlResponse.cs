using System.Collections.Generic;

namespace WorkItemServices.Internal
{
    public class WiqlResponse
    {
        public IEnumerable<WorkItemReference> WorkItems { get; }

        public WiqlResponse(IEnumerable<WorkItemReference> workItems)
        {
            WorkItems = workItems;
        }
    }
}
