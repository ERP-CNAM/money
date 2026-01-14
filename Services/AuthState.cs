using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MoneyApp.Services;

public sealed class AuthState
{
    private const string JwtKey = "MoneyApp.Jwt";
    private readonly ProtectedSessionStorage _store;

    public AuthState(ProtectedSessionStorage store)
    {
        _store = store;
    }

    public async Task SetJwtAsync(string jwt)
        => await _store.SetAsync(JwtKey, jwt);

    public async Task<string?> GetJwtAsync()
    {
        var res = await _store.GetAsync<string>(JwtKey);
        return res.Success ? res.Value : null;
    }

    public async Task ClearAsync()
        => await _store.DeleteAsync(JwtKey);
}
