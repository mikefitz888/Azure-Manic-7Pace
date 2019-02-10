using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingService.Internal;

namespace TimeTrackingService
{
    public interface ITimeTrackingClient
    {
        Task<WorkLog> CreateWorkLog(CreateWorkLogRequest createWorkLogRequest);
        Task DeleteWorkLog(string id);
        Task<Me> GetMe();
        Task<IEnumerable<WorkLog>> GetWorkLogs(DateTime from, DateTime to, int skip = 0);
    }
}