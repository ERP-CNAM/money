using MoneyApp.Models;

namespace MoneyApp.Mappers
{
    public static class InvoiceMapper
    {
        public static InvoiceDto ToInvoiceDto(this InvoiceApiDto api)
        {
            return new InvoiceDto
            {
                Ref_Facture = api.InvoiceRef,
                Facture_Date = api.BillingDate,
                Client_Nom = $"{api.Subscription.User.FirstName} {api.Subscription.User.LastName}",
                Facture_Montant = api.AmountInclVat
            };
        }
    }
}
