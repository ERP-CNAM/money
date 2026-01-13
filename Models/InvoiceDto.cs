namespace MoneyApp.Models;

public sealed class InvoiceDto
{
    public DateOnly Facture_Date { get; set; }
    public string Ref_Facture { get; set; } = "";
    public string Client_Id { get; set; } = "";
    public string Client_Nom { get; set; } = "";
    public decimal Facture_Montant { get; set; }
}
