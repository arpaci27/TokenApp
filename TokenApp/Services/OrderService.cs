namespace TokenOrderApp.Services;

public class OrderService
{
    private readonly TokenService _tokenService;

    public OrderService(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task GetOrdersAsync()
    {
        var token = await _tokenService.GetTokenAsync();

        // Gerçek istek yerine dosyadan okuma
        var json = await File.ReadAllTextAsync("orders.json");

        Console.WriteLine($"[{DateTime.Now}] Siparişler (Token: {token}):\n{json}\n");
    }
}
