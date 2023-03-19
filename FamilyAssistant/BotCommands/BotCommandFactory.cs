using FamilyAssistant.Constants;
using FamilyAssistant.Interfaces.Commands;

namespace FamilyAssistant.BotCommands;

public class BotCommandFactory : IBotCommandFactory
{
    private readonly IServiceProvider _serviceProvider;

    public BotCommandFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IBotCommand GetBotCommand(string command) =>
        command switch
        {
            Commands.BuyProductCommand => _serviceProvider.GetRequiredService<BuyProductBotCommand>(),
            Commands.GetProductsToBuyCommand => _serviceProvider.GetRequiredService<GetProductsToBuyBotCommand>(),
            _ => throw new NotImplementedException("Unknown command"),
        };

    public IBotQueryCommand GetBotQueryCommand(string command) =>
        command switch
        {
            Commands.ToggleBuyProductQueryCommand => _serviceProvider.GetRequiredService<ToggleBuyProductQueryCommand>(),
            _ => throw new NotImplementedException("Unknown command"),
        };
}
