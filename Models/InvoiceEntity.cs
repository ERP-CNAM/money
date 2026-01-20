namespace MoneyApp.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("invoices")]
    public class InvoiceEntity
    {
        [Key]
        public int Id { get; set; }

        [Column("facture_date")]
        public DateOnly FactureDate { get; set; }

        [Column("ref_facture")]
        public string RefFacture { get; set; } = "";

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("client_nom")]
        public string ClientNom { get; set; } = "";

        [Column("facture_montant")]
        public decimal FactureMontant { get; set; }
    }
}
