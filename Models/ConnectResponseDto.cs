namespace MoneyApp.Models
{
    public class ConnectResponseDto<T>
    {
        public bool Success { get; set; }
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";
        public List<T> Payload { get; set; } = new();
    }
}
