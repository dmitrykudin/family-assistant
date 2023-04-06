using FamilyAssistant.Constants;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.Commands;
using FamilyAssistant.Interfaces.Services;
using FamilyAssistant.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FamilyAssistant.BotCommands;

public class BuyProductBotCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IProductToBuyService _productToBuyService;

    private readonly char[] _numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public BuyProductBotCommand(ITelegramBotClient botClient, IProductToBuyService productToBuyService)
    {
        _botClient = botClient;
        _productToBuyService = productToBuyService;
    }

    public async Task ExecuteAsync(
        Message message,
        CancellationToken token)
    {
        string productNames;
        if (message.Text!.StartsWith('/'))
        {
            var tokens = message.Text!.Split(' ');
            productNames = string.Join(' ', tokens.Skip(1));
        }
        else
        {
            productNames = message.Text;
        }

        if (productNames.IsNullOrEmpty())
        {
            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                Messages.TypeWhatYouWantToBuy,
                replyToMessageId: message.MessageId,
                replyMarkup: new ForceReplyMarkup
                {
                    InputFieldPlaceholder = Messages.TypeWhatYouWantToBuy,
                },
                cancellationToken: token);
        }
        else
        {
            var products = productNames
                .Split('\n')
                .Where(x => !x.IsNullOrEmpty())
                .Select(x =>
                {
                    if (!x.Any(char.IsDigit))
                    {
                        return new AddProductToBuyDto { Name = x };
                    }

                    var splitIndex = x.IndexOfAny(_numbers);

                    return new AddProductToBuyDto
                    {
                        Name = x.Substring(0, splitIndex).Trim(),
                        Quantity = x.Substring(splitIndex, x.Length - splitIndex),
                    };
                })
                .ToArray();

            await _productToBuyService.AddProductsToBuy(products, token);

            await _botClient.SendTextMessageAsync(
                message.Chat.Id,
                Messages.MarkedThatINeedToBuyThis(
                    string.Join(", ", products.Select(x => Messages.AddProductText(x)))),
                replyToMessageId: message.MessageId,
                cancellationToken: token);
        }
    }
}
