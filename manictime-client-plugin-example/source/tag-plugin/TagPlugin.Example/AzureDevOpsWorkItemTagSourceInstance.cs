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
                var config = CreateConfigFrom(azureDevOpsWorkItemTagSettings);

                _tagsImporter = new TagsImporter(config);
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

            TagsImporterConfig config = CreateConfigFrom(azureDevOpsSettings);

            _tagsImporter = new TagsImporter(config);
        }

        protected override void Update()
        {
            if (_tagsImporter == null)
            {
                return;
            }

            TagSourceItem[] tags = _tagsImporter
                .GetTagsAsync()
                .Result
                .ToArray();

            TagSourceCache.Update(InstanceId, tags, null, false);
        }

        private static TagsImporterConfig CreateConfigFrom(AzureDevOpsWorkItemTagSettings settings)
        {
            return new TagsImporterConfig
            {
                Organization = settings.Organization,
                PersonalAccessToken = settings.PersonalAccessToken,
                TimeTrackingToken = settings.TimeTrackerApiSecret,

                BillableQueryTemplate = settings.BillableWiqlQueryTemplate,
                NonBillableQueryTemplate = settings.NonBillableWiqlQueryTemplate
            };
        }
    }
}