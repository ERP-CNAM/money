using MoneyApp.Mappers;

namespace MoneyApp.Services
{
    public class DataSyncService
    {
        private readonly ExternalConnectService _external;
        private readonly ImportService _import;
        private readonly ExportService _export;
        private readonly IWebHostEnvironment _env;

        public DataSyncService(
            ExternalConnectService external,
            ImportService import,
            ExportService export,
            IWebHostEnvironment env)
        {
            _external = external;
            _import = import;
            _export = export;
            _env = env;
        }

        public async Task SyncAsync()
        {
            await SyncInvoicesAsync();
            await ExportDatabaseAsync();
        }

        public async Task SyncInvoicesAsync()
        {
            Console.WriteLine("IMPORT");
            // API → DTO API
            var apiInvoices = await _external.FetchInvoicesAsync();

            // Mapping
            var invoices = apiInvoices
                .Select(i => i.ToInvoiceDto())
                .ToList();

            // DB
            await _import.ImportInvoicesAsync(invoices);

            // DB → JSON
            var invoicesPath = Path.Combine(_env.ContentRootPath, "Data", "invoices.json");
            var paymentsPath = Path.Combine(_env.ContentRootPath, "Data", "payments.json");

            await _export.ExportAllAsync(invoicesPath, paymentsPath);
        }
        private async Task ExportDatabaseAsync()
        {
            Console.WriteLine("EXPORT");
            var invoicesPath = Path.Combine(_env.ContentRootPath, "Data", "invoices.json");
            var paymentsPath = Path.Combine(_env.ContentRootPath, "Data", "payments.json");

            await _export.ExportAllAsync(invoicesPath, paymentsPath);
        }
    }
}
