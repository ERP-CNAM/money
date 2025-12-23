namespace MoneyApp.Models;

public sealed class InvoiceDto
{
    public DateOnly FactureDate { get; set; }
    public string RefFacture { get; set; } = "";
    public string ClientId { get; set; } = "";
    public string ClientNom { get; set; } = "";
    public decimal FactureMontant { get; set; }
}
