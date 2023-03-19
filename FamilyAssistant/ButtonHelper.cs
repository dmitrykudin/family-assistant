using FamilyAssistant.Constants;
using FamilyAssistant.Extensions;
using FamilyAssistant.Models;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace FamilyAssistant;

public static class ButtonHelper
{
    public static InlineKeyboardMarkup? GetProductsToBuyButtons(ProductToBuyDto[] products)
    {
        if (products.IsNullOrEmpty())
        {
            return null;
        }

        return new InlineKeyboardMarkup(products
            .Batch(2)
            .Select(x => x
                .Select(y => InlineKeyboardButton
                    .WithCallbackData(
                        Messages.ProductButtonText(y),
                        JsonConvert.SerializeObject(new MarkBoughtProductDto
                        {
                            Id = y.Id,
                            Command = Commands.ToggleBuyProductQueryCommand,
                        })))));
    }
}
