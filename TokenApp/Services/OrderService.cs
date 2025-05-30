using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace TokenOrderApp.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;
    private readonly IConfiguration _config;

    public OrderService(HttpClient httpClient, TokenService tokenService, IConfiguration config)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task GetOrdersAsync()
    {
        var token = await _tokenService.GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Split(' ')[1]);

        var response = await _httpClient.GetAsync(_config["ApiSettings:OrderUrl"]);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"[{DateTime.Now}] Siparişler: {content}");
    }
}
