using Finkit.ManicTime.Common.TagSources;

namespace TagPlugin.Settings
{
    public class AzureDevOpsWorkItemTagSettings : ITagSourceSettings
    {
        public string Organization { get; set; } = "chorussolutions";

        public string BillableActivityId { get; set; } = "990971f5-00cb-418f-a3cd-fbff240e0342";

        public string NonBillableActivityId { get; set; } = "0e585326-4d4c-43bb-80fc-835862ff69e4";

        public string BillableWiqlQueryTemplate { get; set; } =
            $@" SELECT [System.Id]
                FROM WorkItems
                WHERE [System.WorkItemType] = 'Task'
                    and [System.ChangedDate] > @today - 30
                    and [System.AssignedTo] = '@UniqueName'
                    and [System.State] <> 'Removed'
                ORDER BY [Microsoft.VSTS.Common.StateChangeDate] DESC";

        public string NonBillableWiqlQueryTemplate { get; set; } =
            $@" SELECT [System.Id]
                FROM WorkItems
                WHERE [System.WorkItemType] <> ''
                    and [System.State] <> 'Removed'
                    and [System.Id] in (22139, 27708, 28034, 27709, 27710)
                ORDER BY [Microsoft.VSTS.Scheduling.CompletedWork] DESC";

        public string PersonalAccessToken { get; set; }

        public string TimeTrackerApiSecret { get; set; }
    }
}