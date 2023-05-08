using FamilyAssistant.Constants;
using FamilyAssistant.Extensions;
using FamilyAssistant.Helpers;
using FamilyAssistant.Interfaces.Commands;
using FamilyAssistant.Interfaces.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FamilyAssistant.BotCommands;

public class GetProductsToBuyBotCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IProductToBuyService _productToBuyService;

    public GetProductsToBuyBotCommand(ITelegramBotClient botClient, IProductToBuyService productToBuyService)
    {
        _botClient = botClient;
        _productToBuyService = productToBuyService;
    }

    public async Task ExecuteAsync(Message message, CancellationToken token)
    {
        var products = await _productToBuyService.GetProductsToBuy(token);

        var text = products.IsNullOrEmpty()
            ? Messages.YouHaveNoProductsToBuy
            : Messages.HereAreYourProductsToBuy;

        await _botClient.SendTextMessageAsync(
            message.Chat.Id,
            text,
            parseMode: ParseMode.Html,
            replyToMessageId: message.MessageId,
            replyMarkup: ButtonHelper.GetProductsToBuyButtons(products),
            cancellationToken: token);
    }
}
