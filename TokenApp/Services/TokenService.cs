using System.Text.Json;
using TokenApp.Models;

namespace TokenOrderApp.Services;

public class TokenService
{
    private TokenResponse? _cachedToken;
    private DateTime _expiry;

    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken != null && DateTime.UtcNow < _expiry)
            return $"{_cachedToken.token_type} {_cachedToken.access_token}";

        var json = await File.ReadAllTextAsync("token-response.json");
        _cachedToken = JsonSerializer.Deserialize<TokenResponse>(json);
        _expiry = DateTime.UtcNow.AddSeconds(_cachedToken.expires_in - 60); // 1 dk erken expire

        return $"{_cachedToken.token_type} {_cachedToken.access_token}";
    }
}
