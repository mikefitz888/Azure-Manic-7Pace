using Newtonsoft.Json;

namespace WorkItemServices.Internal
{
    public class WorkItemFields
    {
        /*
         * Constructor initialization can't be used by
         * Json.net here as some of the properties include
         * illegal characters. e.g. System.Id
         * 
         * Mark added properties with JsonProperty("<name>")
         * attribute instead, and include a private setter.
         */

        [JsonProperty("System.Id")]
        public int Id { get; private set; }

        [JsonProperty("System.AreaPath")]
        public string AreaPath { get; private set; }

        [JsonProperty("System.TeamProject")]
        public string TeamProject { get; private set; }

        [JsonProperty("System.Title")]
        public string Title { get; private set; }

        [JsonProperty("System.WorkItemType")]
        public string WorkItemType { get; private set; }
    }
}
