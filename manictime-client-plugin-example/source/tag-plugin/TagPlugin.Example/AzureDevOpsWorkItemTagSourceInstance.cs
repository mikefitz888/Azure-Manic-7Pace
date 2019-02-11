using Finkit.ManicTime.Common.TagSources;
using TagPlugin.ImportTags;
using TagPlugin.Settings;

namespace TagPlugin
{
    public class AzureDevOpsWorkItemTagSourceInstance : BasicTagSourceInstance
    {
        private TagsImporter _tagsImporter;

        public AzureDevOpsWorkItemTagSourceInstance(AzureDevOpsWorkItemTagSettings azureDevOpsWorkItemTagSettings)
        {
            if (string.IsNullOrWhiteSpace(azureDevOpsWorkItemTagSettings.PersonalAccessToken) == false)
            {
                _tagsImporter = new TagsImporter(
                organization: azureDevOpsWorkItemTagSettings.Organization,
                personalAccessToken: azureDevOpsWorkItemTagSettings.PersonalAccessToken,
                timeTrackingToken: azureDevOpsWorkItemTagSettings.TimeTrackerApiSecret,
                wiqlQueryTemplate: azureDevOpsWorkItemTagSettings.WiqlQueryTemplate);
            }            
        }

        public override void UpdateSettings(ITagSourceSettings settings)
        {
            var azureDevOpsSettings = (AzureDevOpsWorkItemTagSettings)settings ?? new AzureDevOpsWorkItemTagSettings();

            if (string.IsNullOrWhiteSpace(azureDevOpsSettings.PersonalAccessToken))
            {
                _tagsImporter = null;
                return;
            }

            _tagsImporter = new TagsImporter(
                organization: azureDevOpsSettings.Organization,
                personalAccessToken: azureDevOpsSettings.PersonalAccessToken,
                timeTrackingToken: azureDevOpsSettings.TimeTrackerApiSecret,
                wiqlQueryTemplate: azureDevOpsSettings.WiqlQueryTemplate);
        }

        protected override void Update()
        {
            if (_tagsImporter == null)
            {
                return;
            }

            TagSourceItem[] tags = _tagsImporter
                .GetTags()
                .Result
                .ToArray();

            TagSourceCache.Update(InstanceId, tags, null, false);
        }
    }
}