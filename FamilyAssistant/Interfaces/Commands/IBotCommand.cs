using Telegram.Bot.Types;

namespace FamilyAssistant.Interfaces.Commands;

public interface IBotCommand
{
    Task ExecuteAsync(Message message, CancellationToken token);
}
