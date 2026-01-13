namespace MoneyApp.Models;

public sealed class PaymentDto
{
    public DateOnly Paiement_Date { get; set; }
    public string Facture_Ref { get; set; } = "";
    public decimal Facture_Montant { get; set; }
    public string Moyen_Paiement { get; set; } = ""; // PRELEVEMENT / CB
}
