namespace MoneyApp.Models;

public sealed class BankStatementRow
{
    public DateOnly Date { get; set; }
    public string Ref { get; set; } = "";
    public string Description { get; set; } = "Prélèvement";
    public string Nom { get; set; } = "";
    public decimal Montant { get; set; }
    public string Statut { get; set; } = "UNKNOWN";
}
