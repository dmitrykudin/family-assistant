using FamilyAssistant.Interfaces.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FamilyAssistant.Services.UpdateHandlers;

public class PollingUpdateHandler : UpdateHandlerBase, IUpdateHandler
{
    public PollingUpdateHandler(ITelegramBotClient botClient, IBotCommandFactory botCommandFactory)
        : base(botClient, botCommandFactory)
    {
    }

    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        await base.HandleUpdateAsync(update, cancellationToken);
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
