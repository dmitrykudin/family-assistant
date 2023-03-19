using Telegram.Bot.Types;

namespace FamilyAssistant.Interfaces.Commands;

public interface IBotQueryCommand
{
    Task ExecuteAsync(CallbackQuery callbackQuery, CancellationToken token);
}
