using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace MoneyApp.Services;

public sealed class ConnectGateway
{
    private readonly HttpClient _http;
    private readonly IConfiguration _cfg;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ConnectGateway(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;
    }

    public async Task<ConnectResponse<T>> PostAsync<T>(
        string serviceName,
        string path,
        object payload,
        string? jwt = null,
        CancellationToken ct = default)
    {
        var baseUrl = Environment.GetEnvironmentVariable("CONNECT_URL");
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return ConnectResponse<T>.Fail("Connect:BaseUrl non configurée");
        }

        var url = new Uri(new Uri(baseUrl), "/connect");

        var request = new ConnectRequest
        {
            ClientName = _cfg["Connect:ClientName"] ?? "money",
            ClientVersion = _cfg["Connect:ClientVersion"] ?? "1.0.0",
            ServiceName = serviceName,
            Path = path,
            Debug = false,
            Payload = payload
        };

        var json = JsonSerializer.Serialize(request);

        using var msg = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (!string.IsNullOrWhiteSpace(jwt))
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        HttpResponseMessage resp;
        try
        {
            resp = await _http.SendAsync(msg, ct);
        }
        catch (Exception ex)
        {
            return ConnectResponse<T>.Fail($"CONNECT inaccessible: {ex.Message}");
        }

        var raw = await resp.Content.ReadAsStringAsync(ct);

        Console.WriteLine("=== CONNECT RAW ===");
        Console.WriteLine(raw);
        Console.WriteLine("===================");

        try
        {
            var envelope = JsonSerializer.Deserialize<T>(raw, JsonOptions);
            if (envelope is null)
                return ConnectResponse<T>.Fail("Réponse CONNECT vide");

            return ConnectResponse<T>.Ok(envelope);
        }
        catch (Exception ex)
        {
            return ConnectResponse<T>.Fail($"Désérialisation impossible: {ex.Message}");
        }
    }

    private sealed class ConnectRequest
    {
        [JsonPropertyName("clientName")] public string ClientName { get; set; } = "";
        [JsonPropertyName("clientVersion")] public string ClientVersion { get; set; } = "";
        [JsonPropertyName("serviceName")] public string ServiceName { get; set; } = "";
        [JsonPropertyName("path")] public string Path { get; set; } = "";
        [JsonPropertyName("debug")] public bool Debug { get; set; }
        [JsonPropertyName("payload")] public object Payload { get; set; } = new();
    }
}

public sealed class ConnectResponse<T>
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public T? Payload { get; set; }

    public static ConnectResponse<T> Ok(T payload) => new() { Success = true, Payload = payload };
    public static ConnectResponse<T> Fail(string error) => new() { Success = false, Error = error };
}
