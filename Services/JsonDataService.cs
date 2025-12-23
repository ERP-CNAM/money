using System.Text.Json;
using MoneyApp.Models;

namespace MoneyApp.Services;

public sealed class JsonDataService
{
    private readonly IWebHostEnvironment _env;

    public JsonDataService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<List<InvoiceDto>> LoadInvoicesAsync()
    {
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
