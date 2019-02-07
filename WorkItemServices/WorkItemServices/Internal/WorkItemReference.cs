namespace WorkItemServices.Internal
{
    public class WorkItemReference
    {
        public int Id { get; }

        public string Url { get; }

        public WorkItemReference(int id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}
