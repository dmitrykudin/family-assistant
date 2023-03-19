using FamilyAssistant.Interfaces.Commands;
using Telegram.Bot;

namespace FamilyAssistant.Services.UpdateHandlers;

public class WebHookUpdateHandler : UpdateHandlerBase
{
    public WebHookUpdateHandler(ITelegramBotClient botClient, IBotCommandFactory botCommandFactory)
        : base(botClient, botCommandFactory)
    {
    }
}
