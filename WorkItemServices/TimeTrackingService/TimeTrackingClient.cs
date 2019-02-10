using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using HttpExtensions;
using TimeTrackingService.Internal;

namespace TimeTrackingService
{
    public class TimeTrackingClient : ITimeTrackingClient
    {
        private const string _apiVersion = "3.0-preview";

        private readonly string _bearerToken;

        private readonly string _baseUrl = "https://chorussolutions.timehub.7pace.com/api/rest/";

        public TimeTrackingClient(string timeTrackerToken)
        {
            _bearerToken = timeTrackerToken;
        }

        public async Task<WorkLog> CreateWorkLog(CreateWorkLogRequest createWorkLogRequest)
        {
            Url url = _baseUrl
                .AppendPathSegment("workLogs")
                .SetQueryParam("api-version", _apiVersion);

            var response = await url
                .WithOAuthBearerToken(_bearerToken)
                .PostJsonAsync(createWorkLogRequest)
                .ReceiveJson<ApiResponse<WorkLog>>();

            return response.Data;
        }

        public async Task<IEnumerable<WorkLog>> GetWorkLogs(DateTime from, DateTime to, int skip = 0)
        {
            Url url = _baseUrl
                .AppendPathSegment("workLogs")
                .SetQueryParams(new Dictionary<string, object>
                {
                    { "api-version", _apiVersion },
                    { "$fromTimestamp", from },
                    { "$toTimestamp", to },
                    { "$count", 100 },
                    { "$skip", skip }
                });

            ApiResponse<IEnumerable<WorkLog>> workLogResponse = await url
                .WithOAuthBearerToken(_bearerToken)
                .GetJsonAsync<ApiResponse<IEnumerable<WorkLog>>>();

            return workLogResponse.Data;
        }

        public async Task DeleteWorkLog(string id)
        {
            Url url = _baseUrl
                .AppendPathSegments("workLogs", id)
                .SetQueryParam("api-version", _apiVersion);

            await url
                .WithOAuthBearerToken(_bearerToken)
                .DeleteAsync();
        }

        public async Task<Me> GetMe()
        {
            Url url = _baseUrl
                .AppendPathSegment("me")
                .SetQueryParam("api-version", _apiVersion);

            ApiResponse<Me> response = await url
                .WithOAuthBearerToken(_bearerToken)
                .GetJsonAsync<ApiResponse<Me>>();

            return response.Data;
        }
    }
}
