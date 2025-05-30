using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TokenApp.Models;

namespace TokenOrderApp.Services;

public class TokenService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private TokenResponse? _cachedToken;
    private DateTime _expiry;

    public TokenService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken != null && DateTime.UtcNow < _expiry)
            return $"{_cachedToken.token_type} {_cachedToken.access_token}";

        var form = new Dictionary<string, string>
        {
            {"client_id", _config["ApiSettings:ClientId"]},
            {"client_secret", _config["ApiSettings:ClientSecret"]},
            {"grant_type", "client_credentials"}
        };

        var response = await _httpClient.PostAsync(_config["ApiSettings:TokenUrl"], new FormUrlEncodedContent(form));
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        _cachedToken = JsonSerializer.Deserialize<TokenResponse>(json);
        _expiry = DateTime.UtcNow.AddSeconds(_cachedToken.expires_in - 60); // 1 dk erken bitsin

        return $"{_cachedToken.token_type} {_cachedToken.access_token}";
    }
}
