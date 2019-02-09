using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using HttpExtensions;
using WorkItemServices.Internal;

namespace WorkItemServices
{
    public class WorkItemClient
    {
        private readonly string _personalAccessToken, _organization, _baseUrl;

        public WorkItemClient(
            string personalAccessToken,
            string organization = "chorussolutions",
            string baseUrl = "https://dev.azure.com/")
        {
            _personalAccessToken = personalAccessToken;
            _organization = organization;
            _baseUrl = baseUrl;
        }

        public async Task<WiqlResponse> GetAssignedWorkItemReferences()
        {
            const string name = "Michael Fitzpatrick";

            Url url = _baseUrl
                .AppendPathSegments(_organization, "_apis/wit/wiql")
                .SetQueryParam("api-version", "5.0");


            WiqlResponse response = await url
               .WithBasicAuth("", _personalAccessToken)
               .PostJsonAsync(new
               {
                   query = $@"Select [System.Id], [System.Title], [System.State] From WorkItems Where [System.WorkItemType] = 'Task' AND [System.AssignedTo] = '{name}' AND [State] <> 'Closed' AND [State] <> 'Removed' order by [Microsoft.VSTS.Common.Priority] asc, [System.CreatedDate] desc"
               })
               .ReceiveJson<WiqlResponse>();

            return response;
        }

        public async Task<IEnumerable<WorkItem>> GetWorkItemsByReference(IEnumerable<WorkItemReference> workItemReferences)
        {

            Url url = _baseUrl
                .AppendPathSegments(_organization, "_apis/wit/workitemsbatch")
                .SetQueryParam("api-version", "5.0");


            WorkItemsResponse response = await url
                .WithBasicAuth("", _personalAccessToken)
                .PostJsonAsync(WorkItemsRequest.ConstructFrom(workItemReferences))
                .ReceiveJson<WorkItemsResponse>();

            return response.Value;
        }
    }
}
