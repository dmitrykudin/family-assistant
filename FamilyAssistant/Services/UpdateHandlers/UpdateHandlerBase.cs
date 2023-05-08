using FamilyAssistant.Constants;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.Commands;
using FamilyAssistant.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FamilyAssistant.Services.UpdateHandlers;

public class UpdateHandlerBase
{
    protected readonly ITelegramBotClient _botClient;
    private readonly IBotCommandFactory _botCommandFactory;

    public UpdateHandlerBase(ITelegramBotClient botClient, IBotCommandFactory botCommandFactory)
    {
        _botClient = botClient;
        _botCommandFactory = botCommandFactory;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken token)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, token),
            { EditedMessage: { } editedMessage } => BotOnMessageReceived(editedMessage, token),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, token),
            _ => BotOnUnknownUpdateReceived(update, token),
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken token)
    {
        if (message.Text.IsNullOrEmpty())
        {
            return;
        }

        var command = IsCommand(message)
            ? message.Text!.Split(' ')[0]
            : message.ReplyToMessage?.Text == Messages.TypeWhatYouWantToBuy
                ? Commands.BuyProductCommand
                : null;

        if (command.IsNullOrEmpty())
        {
            return;
        }

        var botCommand = _botCommandFactory.GetBotCommand(command!);

        if (botCommand is null)
        {
            return;
        }

        await botCommand.ExecuteAsync(message, token);
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken token)
    {
        CommandCallbackBaseDto? commandCallback;

        try
        {
            commandCallback = JsonConvert.DeserializeObject<CommandCallbackBaseDto>(callbackQuery.Data!);
        }
        catch (JsonException)
        {
            commandCallback = null;
        }

        if (commandCallback is null || commandCallback.Command.IsNullOrEmpty())
        {
            await _botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                Messages.SorryICantDoThisYet,
                cancellationToken: token);
        }

        var botQueryCommand = _botCommandFactory.GetBotQueryCommand(commandCallback!.Command);

        await botQueryCommand.ExecuteAsync(callbackQuery, token);
    }

    private async Task BotOnUnknownUpdateReceived(Update update, CancellationToken token)
    {
        await _botClient.SendTextMessageAsync(
            update.Message!.Chat.Id,
            Messages.SorryICantDoThisYet,
            cancellationToken: token);
    }

    private static bool IsCommand(Message message) =>
        !message.Text.IsNullOrEmpty() && message.Text!.StartsWith('/');
}
