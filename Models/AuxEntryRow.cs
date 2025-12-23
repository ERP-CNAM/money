namespace MoneyApp.Models;

public sealed class AuxEntryRow
{
    public string Compte { get; set; } = "411";
    public DateOnly Date { get; set; }
    public string Nom { get; set; } = "";
    public string Ref { get; set; } = "";
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}
