using Finkit.ManicTime.Common.TagSources;
using TagPlugin.ImportTags;
using TagPlugin.Settings;

namespace TagPlugin
{
    public class AzureDevOpsWorkItemTagSourceInstance : BasicTagSourceInstance
    {
        private string _organization;

        private string _personalAccessToken;

        private string _timeTrackerApiSecret;

        public AzureDevOpsWorkItemTagSourceInstance(AzureDevOpsWorkItemTagSettings azureDevOpsWorkItemTagSettings)
        {
            UpdateSettings(azureDevOpsWorkItemTagSettings);
        }

        public override void UpdateSettings(ITagSourceSettings settings)
        {
            var azureDevOpsSettings = (AzureDevOpsWorkItemTagSettings)settings ?? new AzureDevOpsWorkItemTagSettings();

            _organization = azureDevOpsSettings.Organization;

            _personalAccessToken = azureDevOpsSettings.PersonalAccessToken;

            _timeTrackerApiSecret = azureDevOpsSettings.TimeTrackerApiSecret;
        }

        protected override void Update()
        {
            TagSourceItem[] tags = TagsImporter.GetTags().ToArray();

            TagSourceCache.Update(InstanceId, tags, null, true);
        }
    }
}