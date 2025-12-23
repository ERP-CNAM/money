namespace MoneyApp.Models;

public sealed class GeneralEntryRow
{
    public DateOnly Date { get; set; }
    public string Compte { get; set; } = "";
    public string Description { get; set; } = "";
    public string Ref { get; set; } = "";
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
}
