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
            var requestObj = new
            {
                apiKey = "changethis",
                clientName = "money",
                clientVersion = "",
                serviceName = "back",
                path = "/invoices",
                debug = true,
                payload = ""
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8000/connect")
            {
                Content = JsonContent.Create(requestObj)
            };

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ConnectResponseDto<InvoiceApiDto>>();
            return result?.Payload ?? new();
        }

        public async Task<List<PaymentApiDto>> FetchPaymentsAsync()
        {
            var requestObj = new
            {
                apiKey = "changethis",
                clientName = "money",
                clientVersion = "",
                serviceName = "back",
                path = "/invoices?status=PAID",
                debug = true,
                payload = ""
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8000/connect")
            {
                Content = JsonContent.Create(requestObj)
            };

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ConnectResponseDto<PaymentApiDto>>();
            return result?.Payload ?? new();
        }
    }
}
