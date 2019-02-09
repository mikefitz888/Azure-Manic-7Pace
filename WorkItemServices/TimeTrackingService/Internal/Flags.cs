namespace TimeTrackingService.Internal
{
    public class Flags
    {
        public bool IsTracked { get; }

        public bool IsManuallyEntered { get; }

        public bool IsChanged { get; }

        public bool IsTrackedExtended { get; }

        public bool IsImported { get; }

        public bool IsFromApi { get; }

        public bool IsBillable { get; }

        public Flags(
            bool isTracked,
            bool isManuallyEntered,
            bool isChanged,
            bool isTrackedExtended,
            bool isImported,
            bool isFromApi,
            bool isBillable)
        {
            IsTracked = isTracked;
            IsManuallyEntered = isManuallyEntered;
            IsChanged = isChanged;
            IsTrackedExtended = isTrackedExtended;
            IsImported = isImported;
            IsFromApi = isFromApi;
            IsBillable = isBillable;
        }
    }
}
