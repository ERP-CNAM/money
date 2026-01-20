using Microsoft.EntityFrameworkCore;
using MoneyApp.Data;
using MoneyApp.Models;

namespace MoneyApp.Services
{
    public class ImportService
    {
        private readonly AppDbContext _db;

        public ImportService(AppDbContext db)
        {
            _db = db;
        }

        public async Task ImportInvoicesAsync(List<InvoiceDto> invoices)
        {
            foreach (var dto in invoices)
            {
                var existing = await _db.Invoices
                    .FirstOrDefaultAsync(i => i.RefFacture == dto.Ref_Facture);

                if (existing == null)
                {
                    _db.Invoices.Add(new InvoiceEntity
                    {
                        RefFacture = dto.Ref_Facture,
                        FactureDate = dto.Facture_Date,
                        ClientId = dto.Client_Id,
                        ClientNom = dto.Client_Nom,
                        FactureMontant = dto.Facture_Montant
                    });
                }
                else
                {
                    existing.FactureDate = dto.Facture_Date;
                    existing.ClientId = dto.Client_Id;
                    existing.ClientNom = dto.Client_Nom;
                    existing.FactureMontant = dto.Facture_Montant;
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
