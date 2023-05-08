using FamilyAssistant.Constants;
using FamilyAssistant.Extensions;
using FamilyAssistant.Helpers;
using FamilyAssistant.Interfaces.Commands;
using FamilyAssistant.Interfaces.Services;
using FamilyAssistant.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FamilyAssistant.BotCommands;

public class ToggleBuyProductQueryCommand : IBotQueryCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IProductToBuyService _productToBuyService;

    public ToggleBuyProductQueryCommand(ITelegramBotClient botClient, IProductToBuyService productToBuyService)
    {
        _botClient = botClient;
        _productToBuyService = productToBuyService;
    }

    public async Task ExecuteAsync(CallbackQuery callbackQuery, CancellationToken token)
    {
        var data = JsonConvert.DeserializeObject<MarkBoughtProductDto>(callbackQuery.Data!);

        var product = await _productToBuyService.GetById(data!.Id, token);

        var toggleCommand = product!.IsBought
            ? _productToBuyService.MarkProductToBuy(product.Id, token)
            : _productToBuyService.MarkProductBought(product.Id, token);

        await toggleCommand;

        await _botClient.AnswerCallbackQueryAsync(
            callbackQuery.Id,
            Messages.MarkedThatIBoughtThis(product.Name),
            cancellationToken: token);

        var products = await _productToBuyService.GetProductsToBuy(token);

        var text = products.IsNullOrEmpty()
            ? Messages.YouHaveNoProductsToBuy
            : Messages.HereAreYourProductsToBuy;

        await _botClient.EditMessageTextAsync(
            callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId,
            text,
            parseMode: ParseMode.Html,
            replyMarkup: ButtonHelper.GetProductsToBuyButtons(products),
            cancellationToken: token);
    }
}
