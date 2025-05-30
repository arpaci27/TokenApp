using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TokenOrderApp.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient();
        services.AddSingleton<TokenService>();
        services.AddSingleton<OrderService>();
    });

var app = builder.Build();

var orderService = app.Services.GetRequiredService<OrderService>();

while (true)
{
    try
    {
        await orderService.GetOrdersAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hata: {ex.Message}");
    }

    await Task.Delay(TimeSpan.FromMinutes(5));
}