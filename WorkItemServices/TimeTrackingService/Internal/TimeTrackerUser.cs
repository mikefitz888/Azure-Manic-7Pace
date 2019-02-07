namespace TimeTrackingService.Internal
{
    public class TimeTrackerUser
    {
        public string UniqueName { get; }

        public string VstsId { get; }

        public string VstsCollectionId { get; }

        public string Name { get; }

        public string Id { get; }

        public TimeTrackerUser(string uniqueName, string vstsId, string vstsCollectionId, string name, string id)
        {
            UniqueName = uniqueName;
            VstsId = vstsId;
            VstsCollectionId = vstsCollectionId;
            Name = name;
            Id = id;
        }
    }
}
