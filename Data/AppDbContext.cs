namespace MoneyApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using MoneyApp.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
        public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}
