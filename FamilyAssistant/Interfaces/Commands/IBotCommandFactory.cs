namespace FamilyAssistant.Interfaces.Commands;

public interface IBotCommandFactory
{
    IBotCommand GetBotCommand(string command);

    IBotQueryCommand GetBotQueryCommand(string command);
}
