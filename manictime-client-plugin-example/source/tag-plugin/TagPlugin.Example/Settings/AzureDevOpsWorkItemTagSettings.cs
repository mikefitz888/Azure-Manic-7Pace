using Finkit.ManicTime.Common.TagSources;

namespace TagPlugin.Settings
{
    public class AzureDevOpsWorkItemTagSettings : ITagSourceSettings
    {
        public string Organization { get; set; } = "chorussolutions";

        public string BillableActivityId { get; set; } = "990971f5-00cb-418f-a3cd-fbff240e0342";

        public string NonBillableActivityId { get; set; } = "0e585326-4d4c-43bb-80fc-835862ff69e4";

        public string PersonalAccessToken { get; set; }

        public string TimeTrackerApiSecret { get; set; }
    }
}