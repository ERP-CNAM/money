using MoneyApp.Models;

namespace MoneyApp.Services;

public sealed class AccountingGenerator
{
    // TVA fixe (tu pourras la déplacer dans appsettings plus tard)
    private const decimal TvaRate = 0.20m;

    public (List<GeneralEntryRow> general, List<AuxEntryRow> aux) GenerateFromInvoices(List<InvoiceDto> invoices)
    {
        var general = new List<GeneralEntryRow>();
        var aux = new List<AuxEntryRow>();

        foreach (var inv in invoices)
        {
            var ttc = Round2(inv.FactureMontant);
            var ht = Round2(ttc / (1m + TvaRate));
            var tva = Round2(ttc - ht);

            // 700 Produit (Crédit HT)
            general.Add(new GeneralEntryRow
            {
                Date = inv.FactureDate,
                Compte = "700",
                Description = "Produit",
                Ref = inv.RefFacture,
                Debit = 0,
                Credit = ht
            });

            // 445 TVA (Crédit TVA)
            general.Add(new GeneralEntryRow
            {
                Date = inv.FactureDate,
                Compte = "445",
                Description = "TVA",
                Ref = inv.RefFacture,
                Debit = 0,
                Credit = tva
            });

            // 411 Client (Débit TTC)
            general.Add(new GeneralEntryRow
            {
                Date = inv.FactureDate,
                Compte = "411",
                Description = "Client",
                Ref = inv.RefFacture,
                Debit = ttc,
                Credit = 0
            });

            // Auxiliaire : suivi client Débit TTC
            aux.Add(new AuxEntryRow
            {
                Date = inv.FactureDate,
                Nom = inv.ClientNom,
                Ref = inv.RefFacture,
                Debit = ttc,
                Credit = 0
            });
        }

        return (general, aux);
    }

    public (List<GeneralEntryRow> general, List<AuxEntryRow> aux, List<BankStatementRow> bank)
        GenerateFromPayments(List<PaymentDto> payments, List<InvoiceDto> invoices)
    {
        var general = new List<GeneralEntryRow>();
        var aux = new List<AuxEntryRow>();
        var bank = new List<BankStatementRow>();

        var invoiceByRef = invoices
            .GroupBy(i => i.RefFacture)
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var pay in payments)
        {
            var amount = Round2(pay.FactureMontant);

            // statut & nom client
            string statut;
            string clientNom;

            if (!invoiceByRef.TryGetValue(pay.FactureRef, out var inv))
            {
                statut = "UNKNOWN_INVOICE";
                clientNom = "Inconnu";
            }
            else
            {
                clientNom = inv.ClientNom;

                var invoiceAmount = Round2(inv.FactureMontant);
                if (Math.Abs(invoiceAmount - amount) <= 0.01m)
                    statut = "MATCHED";
                else
                    statut = "AMOUNT_MISMATCH";
            }

            // bank row (toujours affiché)
            bank.Add(new BankStatementRow
            {
                Date = pay.PaiementDate,
                Ref = pay.FactureRef,
                Description = pay.MoyenPaiement,
                Nom = clientNom,
                Montant = amount,
                Statut = statut
            });

            // écritures règlement uniquement si MATCHED
            if (statut != "MATCHED")
                continue;

            general.Add(new GeneralEntryRow
            {
                Date = pay.PaiementDate,
                Compte = "511",
                Description = "Banque",
                Ref = pay.FactureRef,
                Debit = amount,
                Credit = 0
            });

            general.Add(new GeneralEntryRow
            {
                Date = pay.PaiementDate,
                Compte = "411",
                Description = "Client",
                Ref = pay.FactureRef,
                Debit = 0,
                Credit = amount
            });

            aux.Add(new AuxEntryRow
            {
                Date = pay.PaiementDate,
                Nom = clientNom,
                Ref = pay.FactureRef,
                Debit = 0,
                Credit = amount
            });
        }

        return (general, aux, bank);
    }

    private static decimal Round2(decimal x) => Math.Round(x, 2, MidpointRounding.AwayFromZero);
}
