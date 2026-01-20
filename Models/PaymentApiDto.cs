namespace MoneyApp.Models
{
    public class PaymentApiDto
    {
        public string InvoiceRef { get; set; } = "";
        public DateOnly BillingDate { get; set; }

        public decimal AmountInclVat { get; set; }

        public string Status { get; set; } = "";

        public SubscriptionApiDto Subscription { get; set; } = new();
    }
}
