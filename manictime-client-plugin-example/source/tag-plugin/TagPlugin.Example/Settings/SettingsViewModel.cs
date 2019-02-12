using System.Threading.Tasks;
using Finkit.ManicTime.Common.TagSources;

namespace TagPlugin.Settings
{
    public class SettingsViewModel : TagSourceSettingsViewModel
    {
        public override bool ShowStartingTags => false;

        private string _personalAccessToken;

        public string PersonalAccessToken
        {
            get { return _personalAccessToken; }

            set
            {
                if (_personalAccessToken == value)
                {
                    return;
                }

                _personalAccessToken = value;

                OnPropertyChanged("PersonalAccessToken");
            }
        }

        private string _timeTrackerApiSecret;

        public string TimeTrackerApiSecret
        {
            get { return _timeTrackerApiSecret; }

            set
            {
                if (_timeTrackerApiSecret == value)
                {
                    return;
                }

                _timeTrackerApiSecret = value;

                OnPropertyChanged("TimeTrackerApiSecret");
            }
        }

        private string _organization;

        public string Organization
        {
            get { return _organization; }

            set
            {
                if (_organization == value)
                {
                    return;
                }

                _organization = value;

                OnPropertyChanged("Organization");
            }
        }

        private string _billableActivityId;

        public string BillableActivityId
        {
            get { return _billableActivityId; }

            set
            {
                if (_billableActivityId == value)
                {
                    return;
                }

                _billableActivityId = value;

                OnPropertyChanged("BillableActivityId");
            }
        }

        private string _nonBillableActivityId;

        public string NonBillableActivityId
        {
            get { return _nonBillableActivityId; }

            set
            {
                if (_nonBillableActivityId == value)
                {
                    return;
                }

                _nonBillableActivityId = value;

                OnPropertyChanged("NonBillableActivityId");
            }
        }

        private string _billableWiqlQueryTemplate;

        public string BillableWiqlQueryTemplate
        {
            get { return _billableWiqlQueryTemplate; }

            set
            {
                if (_billableWiqlQueryTemplate == value)
                {
                    return;
                }

                _billableWiqlQueryTemplate = value;

                OnPropertyChanged("BillableWiqlQueryTemplate");
            }
        }

        private string _nonBillableWiqlQueryTemplate;

        public string NonBillableWiqlQueryTemplate
        {
            get { return _nonBillableWiqlQueryTemplate; }

            set
            {
                if (_nonBillableWiqlQueryTemplate == value)
                {
                    return;
                }

                _nonBillableWiqlQueryTemplate = value;

                OnPropertyChanged("NonBillableWiqlQueryTemplate");
            }
        }

        public override void Initialize(ITagSourceSettings settings)
        {
            var azureDevOpsTagSettings = (AzureDevOpsWorkItemTagSettings)settings ?? new AzureDevOpsWorkItemTagSettings();

            Settings = azureDevOpsTagSettings;

            PersonalAccessToken = azureDevOpsTagSettings.PersonalAccessToken;

            TimeTrackerApiSecret = azureDevOpsTagSettings.TimeTrackerApiSecret;

            Organization = azureDevOpsTagSettings.Organization;

            BillableActivityId = azureDevOpsTagSettings.BillableActivityId;

            NonBillableActivityId = azureDevOpsTagSettings.NonBillableActivityId;

            BillableWiqlQueryTemplate = azureDevOpsTagSettings.BillableWiqlQueryTemplate;

            NonBillableWiqlQueryTemplate = azureDevOpsTagSettings.NonBillableWiqlQueryTemplate;
        }

        public override Task<bool> BeforeOk()
        {
            return Task.FromResult(true);
        }

        public override Task OnOk()
        {
            var azureDevOpsWorkItemTagSettings = (AzureDevOpsWorkItemTagSettings)Settings;

            azureDevOpsWorkItemTagSettings.TimeTrackerApiSecret = TimeTrackerApiSecret;
            azureDevOpsWorkItemTagSettings.PersonalAccessToken = PersonalAccessToken;
            azureDevOpsWorkItemTagSettings.Organization = Organization;
            azureDevOpsWorkItemTagSettings.BillableActivityId = BillableActivityId;
            azureDevOpsWorkItemTagSettings.NonBillableActivityId = NonBillableActivityId;
            azureDevOpsWorkItemTagSettings.BillableWiqlQueryTemplate = BillableWiqlQueryTemplate;
            azureDevOpsWorkItemTagSettings.NonBillableWiqlQueryTemplate = NonBillableWiqlQueryTemplate;

            return Task.FromResult(true);
        }
    }
}