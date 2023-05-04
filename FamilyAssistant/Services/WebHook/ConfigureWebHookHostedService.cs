using FamilyAssistant.Config;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace FamilyAssistant.Services.WebHook;

public class ConfigureWebHookHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly BotConfiguration _botConfiguration;

    public ConfigureWebHookHostedService(
        IServiceProvider serviceProvider,
        IOptions<BotConfiguration> botConfiguration)
    {
        _serviceProvider = serviceProvider;
        _botConfiguration = botConfiguration.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAddress = $"{_botConfiguration.HostAddress}{_botConfiguration.Route}";

        await botClient.SetWebhookAsync(
            url: webhookAddress,
            secretToken: _botConfiguration.SecretToken,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}
