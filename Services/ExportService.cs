using MoneyApp.Data;




namespace MoneyApp.Services
{
    using MoneyApp.Models;
    using System.Text.Json;
    using MoneyApp.Data;
    using Microsoft.EntityFrameworkCore;

    public class ExportService
    {
        private readonly AppDbContext _db;

        public ExportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task ExportInvoicesAsync(string path)
        {
            var invoices = await _db.Invoices
                .Select(i => new InvoiceDto
                {
                    Ref_Facture = i.RefFacture,
                    Facture_Date = i.FactureDate,
                    Client_Id = i.ClientId,
                    Client_Nom = i.ClientNom,
                    Facture_Montant = i.FactureMontant
                })
                .ToListAsync();

            var json = JsonSerializer.Serialize(invoices, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(path, json);
        }

        public async Task ExportPaymentsAsync(string path)
        {
            var payments = await _db.Payments
                .Select(p => new PaymentDto
                {
                    Facture_Ref = p.FactureRef,
                    Paiement_Date = p.PaiementDate,
                    Facture_Montant = p.FactureMontant,
                    Moyen_Paiement = p.MoyenPaiement
                })
                .ToListAsync();

            var json = JsonSerializer.Serialize(payments, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(path, json);
        }

        public async Task ExportAllAsync(string invoicesPath, string paymentsPath)
        {
            await ExportInvoicesAsync(invoicesPath);
            await ExportPaymentsAsync(paymentsPath);
        }
    }
}
