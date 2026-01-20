namespace MoneyApp.Models
{
    public class SubscriptionApiDto
    {
        public string ContractCode { get; set; } = "";
        public UserApiDto User { get; set; } = new();
    }
}
