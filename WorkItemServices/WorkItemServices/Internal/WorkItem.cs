namespace WorkItemServices.Internal
{
    public class WorkItem
    {
        public int Id { get; }

        public WorkItemFields Fields { get; }

        public WorkItem(int id, WorkItemFields fields)
        {
            Id = id;
            Fields = fields;
        }
    }
}
