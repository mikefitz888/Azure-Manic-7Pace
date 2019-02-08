using Finkit.ManicTime.Common.TagSources;
using TagPlugin.Settings;

namespace TagPlugin
{
    public class AzureDevOpsWorkItemTagSource : BasicTagSource
    {
        public AzureDevOpsWorkItemTagSource(ITagSourceCache tagSourceCache) : base(tagSourceCache)
        {
        }

        protected override BasicTagSourceInstance CreateServerTagSourceInstance(ITagSourceSettings settings,
            string cacheTimestamp)
        {
            return new AzureDevOpsWorkItemTagSourceInstance((AzureDevOpsWorkItemTagSettings)settings);
        }
    }
}