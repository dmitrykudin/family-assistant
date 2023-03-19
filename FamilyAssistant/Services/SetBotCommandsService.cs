using FamilyAssistant.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FamilyAssistant.Services;

public class SetBotCommandsService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;

    public SetBotCommandsService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commands = Commands.CommandToDescriptionMap
            .Select(x => new BotCommand
            {
                Command = x.Key,
                Description = x.Value,
            })
            .ToArray();

        await _botClient.SetMyCommandsAsync(commands, cancellationToken: stoppingToken);
    }
}
