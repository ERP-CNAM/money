namespace MoneyApp.Services;

public sealed class AuthService
{
    private readonly ConnectGateway _connect;
    private readonly AuthState _authState;

    public AuthService(ConnectGateway connect, AuthState authState)
    {
        _connect = connect;
        _authState = authState;
    }

    public async Task<(bool ok, string? error)> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        // BACK: POST /auth/login :contentReference[oaicite:7]{index=7}
        var payloadToBack = new
        {
            email,       // :contentReference[oaicite:8]{index=8}
            password     // :contentReference[oaicite:9]{index=9}
        };

        // ChangeThis: si CONNECT attend un autre "serviceName"
        var res = await _connect.PostAsync<BackLoginApiResponse>(
            serviceName: "back",
            path: "/auth/login",
            payload: payloadToBack,
            jwt: null,
            ct: ct);

        // Cas erreurs connect / format wrapper
        if (!res.Success || res.Payload is null)
        {
            // ChangeThis: message erreur plus précis si CONNECT renvoie autre chose
            return (false, res.Error ?? $"Connexion refusée (status {res.Status}).");
        }

        // BACK renvoie BaseAPIResponse + payload.token 
        if (!res.Payload.Success)
            return (false, res.Payload.Message ?? "Identifiants invalides.");

        var token = res.Payload.Payload?.Token;

        if (string.IsNullOrWhiteSpace(token))
            return (false, "Token manquant dans la réponse du BACK (payload.token attendu).");

        await _authState.SetJwtAsync(token);
        return (true, null);
    }

    // Modèle basé sur BaseAPIResponse + payload(LoginResponse) 
    private sealed class BackLoginApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public BackLoginPayload? Payload { get; set; }
    }

    private sealed class BackLoginPayload
    {
        public string? Token { get; set; } // LoginResponse.token :contentReference[oaicite:12]{index=12}
        // public object? User { get; set; } // possible, mais pas utile maintenant
    }
}
