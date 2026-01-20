using MoneyApp.Models;
using System.Net.Http.Headers;

namespace MoneyApp.Services
{
    public class ExternalConnectService
    {
        private readonly HttpClient _http;

        public ExternalConnectService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<InvoiceApiDto>> FetchInvoicesAsync()
        {
            var url =
                "http://localhost:8000/connect" +
                "?apiKey=changethis" +
                "&clientName=money" +
                "&clientVersion=" +
                "&serviceName=back" +
                "&path=/invoices" +
                "&debug=true"+
                "&payload=";

            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ConnectResponseDto<InvoiceApiDto>>();
            return result?.Payload ?? new();
        }
    }
}
