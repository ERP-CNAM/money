using System.Text.Json;

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
        var payloadToBack = new { email, password };

        // ✅ On demande un JsonElement pour supporter TOUS les formats CONNECT
        var res = await _connect.PostAsync<JsonElement>(
            serviceName: "back",
            path: "/auth/login",
            payload: payloadToBack,
            jwt: null,
            ct: ct);

        if (!res.Success)
            return (false, res.Error ?? "Erreur CONNECT.");

        if (res.Payload.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
            return (false, "Réponse CONNECT vide.");

        var root = res.Payload;

        // -----------------------------
        // FORMAT B (logs CONNECT) :
        // { request: { success, message }, data: { payloadOut: { token } } }
        // -----------------------------
        if (root.TryGetProperty("request", out var requestEl))
        {
            var ok = requestEl.TryGetProperty("success", out var sEl) && sEl.ValueKind == JsonValueKind.True;
            var msg = requestEl.TryGetProperty("message", out var mEl) ? mEl.GetString() : null;

            if (!ok)
                return (false, msg ?? "Identifiants invalides.");

            if (!root.TryGetProperty("data", out var dataEl))
                return (false, "Réponse CONNECT invalide: champ 'data' manquant.");

            if (!dataEl.TryGetProperty("payloadOut", out var payloadOutEl))
                return (false, "Réponse CONNECT invalide: champ 'data.payloadOut' manquant.");

            var token = payloadOutEl.TryGetProperty("token", out var tokenEl) ? tokenEl.GetString() : null;

            if (string.IsNullOrWhiteSpace(token))
                return (false, "Token manquant (data.payloadOut.token).");

            await _authState.SetJwtAsync(token);
            return (true, null);
        }

        // -----------------------------
        // FORMAT A (Postman CONNECT) :
        // { success, status, message, payload: { token, user } }
        // -----------------------------
        if (root.TryGetProperty("success", out var topSuccessEl))
        {
            var ok = topSuccessEl.ValueKind == JsonValueKind.True;
            var msg = root.TryGetProperty("message", out var topMsgEl) ? topMsgEl.GetString() : null;

            if (!ok)
                return (false, msg ?? "Identifiants invalides.");

            if (!root.TryGetProperty("payload", out var payloadEl))
                return (false, "Réponse CONNECT invalide: champ 'payload' manquant.");

            var token = payloadEl.TryGetProperty("token", out var tokenEl) ? tokenEl.GetString() : null;

            if (string.IsNullOrWhiteSpace(token))
                return (false, "Token manquant (payload.token).");

            await _authState.SetJwtAsync(token);
            return (true, null);
        }

        // Si aucun des formats n'est reconnu
        return (false, "Réponse CONNECT inconnue (format non supporté).");
    }
}
