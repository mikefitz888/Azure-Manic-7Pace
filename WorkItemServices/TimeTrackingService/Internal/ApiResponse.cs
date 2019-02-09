namespace TimeTrackingService.Internal
{
    public class ApiResponse<T>
    {
        public T Data { get; }

        public ApiResponse(T data)
        {
            Data = data;
        }
    }
}
