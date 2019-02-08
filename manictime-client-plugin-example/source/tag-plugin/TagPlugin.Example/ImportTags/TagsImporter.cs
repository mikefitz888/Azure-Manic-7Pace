using System.Collections.Generic;
using Finkit.ManicTime.Common.TagSources;
using Finkit.ManicTime.Shared.Tags.Labels;

namespace TagPlugin.ImportTags
{
    public class TagsImporter
    {
        public static List<TagSourceItem> GetTags()
        {
            var tagSourceItems = new List<TagSourceItem>
            {
                new TagSourceItem
                {
                    Tags = new[] { "#29028", "Update Arrow Color", ClientPlugin.HiddenTagLabel },
                    Notes = "Note"
                },
                new TagSourceItem
                {
                    Tags = new[] { "Demo Project 2", "Demo Activity 2", TagLabels.Billable, ClientPlugin.HiddenTagLabel },
                    Notes = "Note 2"
                }
            };

            return tagSourceItems;
        }
    }
}