using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoneyApp.Models
{
    [Table("payments")]
    public class PaymentEntity
    {
        [Key]
        public int Id { get; set; }

        [Column("paiement_date")]
        public DateOnly PaiementDate { get; set; }

        [Column("facture_ref")]
        public string FactureRef { get; set; } = "";

        [Column("facture_montant")]
        public decimal FactureMontant { get; set; }

        [Column("moyen_paiement")]
        public string MoyenPaiement { get; set; } = "";
    }
}
