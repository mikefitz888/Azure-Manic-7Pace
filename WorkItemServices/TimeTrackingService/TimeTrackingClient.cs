using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TimeTrackingService.Internal;

namespace TimeTrackingService
{
    public class TimeTrackingClient
    {
        private readonly string _bearerToken;

        private readonly string _baseUrl = "https://chorussolutions.timehub.7pace.com/api/rest/";

        public TimeTrackingClient()
        {
            FlurlHttp.Configure(settings => {
                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
        }

        public async void CreateWorkLog(CreateWorkLogRequest createWorkLogRequest)
        {
            Url url = _baseUrl
                .AppendPathSegment("workLogs")
                .SetQueryParam("api-version", "3.0-preview");

            await url
                .WithOAuthBearerToken(_bearerToken)
                .PostJsonAsync(createWorkLogRequest);
        }

        public async Task<TimeTrackerUserResponse> GetUser()
        {
            Url url = _baseUrl
                .AppendPathSegment("me")
                .SetQueryParam("api-version", "3.0-preview");

            TimeTrackerUserResponse response = await url
                .WithOAuthBearerToken(_bearerToken)
                .GetJsonAsync<TimeTrackerUserResponse>();

            return response;
        }
    }
}
