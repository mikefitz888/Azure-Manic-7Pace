using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace WorkItemServices.Internal
{
    public class WorkItemsRequest
    {
        [JsonProperty("$expand")]
        public string Expand { get; }

        [JsonProperty("ids")]
        public IEnumerable<int> Ids { get; }

        private WorkItemsRequest(string expand, IEnumerable<int> ids)
        {
            Expand = expand;
            Ids = ids;
        }

        public static WorkItemsRequest ConstructFrom(IEnumerable<WorkItemReference> workItemReferences)
        {
            IEnumerable<int> ids = workItemReferences.Select(item => item.Id);

            return new WorkItemsRequest("all", ids);
        }
    }
}
