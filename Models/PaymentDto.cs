namespace MoneyApp.Models;

public sealed class PaymentDto
{
    public DateOnly PaiementDate { get; set; }
    public string FactureRef { get; set; } = "";
    public decimal FactureMontant { get; set; }
    public string MoyenPaiement { get; set; } = ""; // PRELEVEMENT / CB
}
