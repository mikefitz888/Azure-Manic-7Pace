using TimeTrackingService.Internal;

namespace TimeTrackingService.Mapping
{
    public static class ActivityTypeConfig
    {
        public static Mapping<ActivityType, string> ActivityTypeIdMapping { get; } = new Mapping<ActivityType, string>
        {
            { ActivityType.Development, "990971f5-00cb-418f-a3cd-fbff240e0342" },
            { ActivityType.Internal, "0e585326-4d4c-43bb-80fc-835862ff69e4" },
            { ActivityType.OutOfOffice, "88a0c114-9746-4867-95f8-1811782a6bba" },
            { ActivityType.Planning, "f4cbb022-91b6-4c9c-b9e1-591681a06b91" },
            { ActivityType.PreSales, "ba046b19-863f-4999-b737-a5cba04d1083" },
            { ActivityType.QA, "15e719c9-373d-45fa-9b9e-a94dbd4832d4" },
            { ActivityType.Support, "eb892560-196f-4872-9047-d06b9694a765" },
            { ActivityType.NotSet, "00000000-0000-0000-0000-000000000000" }
        };
    }
}
