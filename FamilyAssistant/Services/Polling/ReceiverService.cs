using FamilyAssistant.Services.UpdateHandlers;
using Telegram.Bot;

namespace FamilyAssistant.Services.Polling;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService : ReceiverServiceBase<PollingUpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        PollingUpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<PollingUpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}
