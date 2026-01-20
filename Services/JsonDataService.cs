using System.Text.Json;
using MoneyApp.Models;

namespace MoneyApp.Services;

public sealed class JsonDataService
{
    private readonly IWebHostEnvironment _env;
    private readonly ExportService _exporter;

    public JsonDataService(IWebHostEnvironment env, ExportService exporter)
    {
        _env = env;
        _exporter = exporter;
    }

    private async Task SyncFromDatabaseAsync()
    {
        var invoicesPath = Path.Combine(_env.ContentRootPath, "Data", "invoices.json");
        var paymentsPath = Path.Combine(_env.ContentRootPath, "Data", "payments.json");

        await _exporter.ExportAllAsync(invoicesPath, paymentsPath);
    }

    public async Task<List<InvoiceDto>> LoadInvoicesAsync()
    {
        await SyncFromDatabaseAsync();

        var path = Path.Combine(_env.ContentRootPath, "Data", "invoices.json");
        var json = await File.ReadAllTextAsync(path);

        // mapping snake_case -> PascalCase
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // DateOnly: on gère via un converter simple
        options.Converters.Add(new DateOnlyJsonConverter());

        return JsonSerializer.Deserialize<List<InvoiceDto>>(json, options) ?? new();
    }

    public async Task<List<PaymentDto>> LoadPaymentsAsync()
    {
        await SyncFromDatabaseAsync();


        var path = Path.Combine(_env.ContentRootPath, "Data", "payments.json");
        var json = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new DateOnlyJsonConverter());

        return JsonSerializer.Deserialize<List<PaymentDto>>(json, options) ?? new();
    }
}
