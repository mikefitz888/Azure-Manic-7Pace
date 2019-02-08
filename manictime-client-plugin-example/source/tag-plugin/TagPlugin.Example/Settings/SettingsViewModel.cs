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

        public override void Initialize(ITagSourceSettings settings)
        {
            var azureDevOpsTagSettings = (AzureDevOpsWorkItemTagSettings)settings ?? new AzureDevOpsWorkItemTagSettings();

            Settings = azureDevOpsTagSettings;

            PersonalAccessToken = azureDevOpsTagSettings.PersonalAccessToken;

            TimeTrackerApiSecret = azureDevOpsTagSettings.TimeTrackerApiSecret;
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

            return Task.FromResult(true);
        }
    }
}