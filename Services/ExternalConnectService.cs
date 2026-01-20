using MoneyApp.Models;
using System.Net.Http.Headers;

namespace MoneyApp.Services
{
    public class ExternalConnectService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;


        public ExternalConnectService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<List<InvoiceApiDto>> FetchInvoicesAsync()
        {
            var requestObj = new
            {
                apiKey = Environment.GetEnvironmentVariable("CONNECT_API_KEY"), 
                clientName = "money",
                clientVersion = "",
                serviceName = "back",
                path = "/invoices",
                debug = true,
                payload = ""
            };

            var request = new HttpRequestMessage(HttpMethod.Get, Environment.GetEnvironmentVariable("CONNECT_URL") +"/connect") 
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
                apiKey = Environment.GetEnvironmentVariable("CONNECT_API_KEY"),
                clientName = "money",
                clientVersion = "",
                serviceName = "back",
                path = "/invoices?status=PAID",
                debug = true,
                payload = ""
            };

            var request = new HttpRequestMessage(HttpMethod.Get, Environment.GetEnvironmentVariable("CONNECT_URL") + "/connect")
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
