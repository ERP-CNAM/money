//Tout ce qui dépend du format exact CONNECT est marqué ChangeThis.

using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace MoneyApp.Services;

public sealed class ConnectGateway
{
    private readonly HttpClient _http;
    private readonly IConfiguration _cfg;

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
        // ChangeThis: URL de CONNECT (ex: http://localhost:8000)
        var connectBaseUrl = _cfg["Connect:BaseUrl"];

        if (string.IsNullOrWhiteSpace(connectBaseUrl) || connectBaseUrl == "ChangeThis")
        {
            return new ConnectResponse<T>
            {
                Success = false,
                Status = 0,
                Error = "ChangeThis: Connect:BaseUrl n'est pas configurée (ex: http://localhost:8000).",
                Payload = default
            };
        }

        if (!Uri.TryCreate(connectBaseUrl, UriKind.Absolute, out var baseUri))
        {
            return new ConnectResponse<T>
            {
                Success = false,
                Status = 0,
                Error = "ChangeThis: Connect:BaseUrl n'est pas une URL absolue valide.",
                Payload = default
            };
        }

        // ChangeThis: endpoint CONNECT (par défaut /connect)
        var connectUri = new Uri(baseUri, "/connect");

        var req = new ConnectRequest
        {
            // ChangeThis: valeurs attendues par CONNECT
            ClientName = _cfg["Connect:ClientName"] ?? "ChangeThis",
            ClientVersion = _cfg["Connect:ClientVersion"] ?? "ChangeThis",
            ServiceName = serviceName,
            Path = path,
            Debug = false,
            Payload = payload
        };

        using var message = new HttpRequestMessage(HttpMethod.Post, connectUri)
        {
            Content = JsonContent.Create(req)
        };

        // JWT si nécessaire sur les routes protégées
        if (!string.IsNullOrWhiteSpace(jwt))
        {
            message.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        }

        HttpResponseMessage resp;
        try
        {
            resp = await _http.SendAsync(message, ct);
        }
        catch (HttpRequestException)
        {
            return new ConnectResponse<T>
            {
                Success = false,
                Status = 0,
                Error = "ChangeThis: Impossible de contacter CONNECT (service arrêté ou URL incorrecte).",
                Payload = default
            };
        }

        ConnectResponse<T>? wrapper;
        try
        {
            wrapper = await resp.Content.ReadFromJsonAsync<ConnectResponse<T>>(cancellationToken: ct);
        }
        catch
        {
            return new ConnectResponse<T>
            {
                Success = false,
                Status = (int)resp.StatusCode,
                Error = "ChangeThis: Réponse CONNECT non désérialisable.",
                Payload = default
            };
        }

        if (wrapper is null)
        {
            return new ConnectResponse<T>
            {
                Success = false,
                Status = (int)resp.StatusCode,
                Error = "ChangeThis: Réponse CONNECT vide ou inattendue.",
                Payload = default
            };
        }

        if (wrapper.Status == 0)
            wrapper.Status = (int)resp.StatusCode;

        return wrapper;
    }


    // ChangeThis: structure requête CONNECT (si champs différents)
    private sealed class ConnectRequest
    {
        public string ClientName { get; set; } = "";
        public string ClientVersion { get; set; } = "";
        public string ServiceName { get; set; } = "";
        public string Path { get; set; } = "";
        public bool Debug { get; set; }
        public object Payload { get; set; } = new();
    }
}

// ChangeThis: structure réponse CONNECT (si champs différents)
public sealed class ConnectResponse<T>
{
    public bool Success { get; set; }
    public int Status { get; set; }
    public string? Error { get; set; }
    public T? Payload { get; set; }
}
