using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using HttpExtensions;
using WorkItemServices.Internal;

namespace WorkItemServices
{
    public class WorkItemClient : IWorkItemClient
    {
        private readonly string _personalAccessToken, _organization, _baseUrl;

        public WorkItemClient(
            string personalAccessToken,
            string organization,
            string baseUrl = "https://dev.azure.com/")
        {
            _personalAccessToken = personalAccessToken;
            _organization = organization;
            _baseUrl = baseUrl;
        }

        public async Task<WiqlResponse> GetAssignedWorkItemReferences(string uniqueName, string queryTemplate)
        {
            Url url = _baseUrl
                .AppendPathSegments(_organization, "_apis/wit/wiql")
                .SetQueryParam("api-version", "5.0");

            string query = queryTemplate.Replace("@UniqueName", uniqueName);


            WiqlResponse response = await url
               .WithBasicAuth("", _personalAccessToken)
               .PostJsonAsync(new
               {
                   query
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
