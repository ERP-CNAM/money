using MoneyApp.Models;

namespace MoneyApp.Mappers
{
    public static class PaymentMapper
    {
        public static PaymentDto ToPaymentDto(this PaymentApiDto api)
        {
            return new PaymentDto
            {
                //TODO
                Facture_Ref = api.InvoiceRef,
                Paiement_Date = api.BillingDate,
                //Client_Nom = $"{api.Subscription.User.FirstName} {api.Subscription.User.LastName}",
                Facture_Montant = api.AmountInclVat,
                Moyen_Paiement = api.Subscription.User.PaymentMethod.Type
            };
        }
    }
}
