namespace MoneyApp.Models
{
    public class InvoiceApiDto
    {
        public string InvoiceRef { get; set; } = "";
        public DateOnly PeriodStart { get; set; }
        public DateOnly PeriodEnd { get; set; }
        public DateOnly BillingDate { get; set; }

        public decimal AmountInclVat { get; set; }
        public decimal AmountExclVat { get; set; }
        public decimal VatAmount { get; set; }

        public string Status { get; set; } = "";

        public SubscriptionApiDto Subscription { get; set; } = new();
    }
}
