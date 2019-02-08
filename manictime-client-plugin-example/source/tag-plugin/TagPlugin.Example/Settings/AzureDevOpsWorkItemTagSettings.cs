using Finkit.ManicTime.Common.TagSources;

namespace TagPlugin.Settings
{
    public class AzureDevOpsWorkItemTagSettings : ITagSourceSettings
    {
        public string Organization { get; set; }

        public string PersonalAccessToken { get; set; }

        public string TimeTrackerApiSecret { get; set; }
    }
}