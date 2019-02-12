namespace TagPlugin.ImportTags
{
    public class TagsImporterConfig
    {
        public string Organization { get; set; }

        public string PersonalAccessToken { get; set; }

        public string TimeTrackingToken { get; set; }

        public string BillableQueryTemplate { get; set; }

        public string NonBillableQueryTemplate { get; set; }
    }
}
