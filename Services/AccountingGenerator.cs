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
            var ttc = Round2(inv.Facture_Montant);
            var ht = Round2(ttc / (1m + TvaRate));
            var tva = Round2(ttc - ht);

            // 700 Produit (Crédit HT)
            general.Add(new GeneralEntryRow
            {
                Date = inv.Facture_Date,
                Compte = "700",
                Description = "Produit",
                Ref = inv.Ref_Facture,
                Debit = 0,
                Credit = ht
            });

            // 445 TVA (Crédit TVA)
            general.Add(new GeneralEntryRow
            {
                Date = inv.Facture_Date,
                Compte = "445",
                Description = "TVA",
                Ref = inv.Ref_Facture,
                Debit = 0,
                Credit = tva
            });

            // 411 Client (Débit TTC)
            general.Add(new GeneralEntryRow
            {
                Date = inv.Facture_Date,
                Compte = "411",
                Description = "Client",
                Ref = inv.Ref_Facture,
                Debit = ttc,
                Credit = 0
            });

            // Auxiliaire : suivi client Débit TTC
            aux.Add(new AuxEntryRow
            {
                Date = inv.Facture_Date,
                Nom = inv.Client_Nom,
                Ref = inv.Ref_Facture,
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
            .GroupBy(i => i.Ref_Facture)
            .ToDictionary(g => g.Key, g => g.First());

        foreach (var pay in payments)
        {
            var amount = Round2(pay.Facture_Montant);

            // statut & nom client
            string statut;
            string clientNom;

            if (!invoiceByRef.TryGetValue(pay.Facture_Ref, out var inv))
            {
                statut = "UNKNOWN_INVOICE";
                clientNom = "Inconnu";
            }
            else
            {
                clientNom = inv.Client_Nom;

                var invoiceAmount = Round2(inv.Facture_Montant);
                if (Math.Abs(invoiceAmount - amount) <= 0.01m)
                    statut = "MATCHED";
                else
                    statut = "AMOUNT_MISMATCH";
            }

            // bank row (toujours affiché)
            bank.Add(new BankStatementRow
            {
                Date = pay.Paiement_Date,
                Ref = pay.Facture_Ref,
                Description = pay.Moyen_Paiement,
                Nom = clientNom,
                Montant = amount,
                Statut = statut
            });

            // écritures règlement uniquement si MATCHED
            if (statut != "MATCHED")
                continue;

            general.Add(new GeneralEntryRow
            {
                Date = pay.Paiement_Date,
                Compte = "511",
                Description = "Banque",
                Ref = pay.Facture_Ref,
                Debit = amount,
                Credit = 0
            });

            general.Add(new GeneralEntryRow
            {
                Date = pay.Paiement_Date,
                Compte = "411",
                Description = "Client",
                Ref = pay.Facture_Ref,
                Debit = 0,
                Credit = amount
            });

            aux.Add(new AuxEntryRow
            {
                Date = pay.Paiement_Date,
                Nom = clientNom,
                Ref = pay.Facture_Ref,
                Debit = 0,
                Credit = amount
            });
        }

        return (general, aux, bank);
    }

    private static decimal Round2(decimal x) => Math.Round(x, 2, MidpointRounding.AwayFromZero);
}
