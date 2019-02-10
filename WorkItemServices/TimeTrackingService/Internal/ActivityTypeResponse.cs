namespace TimeTrackingService.Internal
{
    public class ActivityTypeResponse
    {
        public string Id { get; }

        public string Name { get; }

        public string Color { get; }

        public ActivityTypeResponse(string id, string name, string color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
