using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using HttpExtensions;
using TimeTrackingService.Internal;

namespace TimeTrackingService
{
    public class TimeTrackingClient : ITimeTrackingClient
    {
        private const int PageSize = 100;

        private const string ApiVersion = "3.0-preview";

        private readonly string _bearerToken;

        private readonly string _baseUrl;

        public TimeTrackingClient(string timeTrackerToken, string baseUrl)
        {
            _baseUrl = baseUrl;
            _bearerToken = timeTrackerToken;
        }

        public async Task<WorkLog> CreateWorkLog(CreateWorkLogRequest createWorkLogRequest)
        {
            Url url = _baseUrl
                .AppendPathSegment("workLogs")
                .SetQueryParam("api-version", ApiVersion);

            ApiResponse<WorkLog> response = await url
                .WithOAuthBearerToken(_bearerToken)
                .PostJsonAsync(createWorkLogRequest)
                .ReceiveJson<ApiResponse<WorkLog>>();

            return response.Data;
        }

        public async Task<IEnumerable<WorkLog>> GetWorkLogs(DateTime from, DateTime to)
        {
            int fetchedCount;
            var skip = 0;

            IEnumerable<WorkLog> worklogs = new List<WorkLog>();

            do
            {
                IEnumerable<WorkLog> fetchedWorkLogs = await GetWorkLogsPage(from, to, skip);

                List<WorkLog> fetchedWorkLogsList = fetchedWorkLogs.ToList();

                worklogs = worklogs.Concat(fetchedWorkLogsList);

                fetchedCount = fetchedWorkLogsList.Count;
                skip += PageSize;
            }
            while (fetchedCount == PageSize);

            return worklogs;
        }

        private async Task<IEnumerable<WorkLog>> GetWorkLogsPage(DateTime from, DateTime to, int skip)
        {
            Url url = _baseUrl
                .AppendPathSegment("workLogs")
                .SetQueryParams(new Dictionary<string, object>
                {
                    { "api-version", ApiVersion },
                    { "$fromTimestamp", from },
                    { "$toTimestamp", to },
                    { "$count", PageSize },
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
                .SetQueryParam("api-version", ApiVersion);

            await url
                .WithOAuthBearerToken(_bearerToken)
                .DeleteAsync();
        }

        public async Task<Me> GetMe()
        {
            Url url = _baseUrl
                .AppendPathSegment("me")
                .SetQueryParam("api-version", ApiVersion);

            ApiResponse<Me> response = await url
                .WithOAuthBearerToken(_bearerToken)
                .GetJsonAsync<ApiResponse<Me>>();

            return response.Data;
        }
    }
}
