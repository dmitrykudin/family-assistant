using FamilyAssistant.Constants;
using FamilyAssistant.Enums;
using FamilyAssistant.Extensions;
using FamilyAssistant.Models;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace FamilyAssistant.Helpers;

public static class ButtonHelper
{
    public static InlineKeyboardMarkup? GetProductsToBuyButtons(ProductToBuyDto[] products)
    {
        if (products.IsNullOrEmpty())
        {
            return null;
        }

        var productByCategoriesButtons = products
            .GroupBy(x => x.Product?.Category is not null
                ? x.Product!.Category
                : ProductCategory.None)
            .OrderBy(x => x.Key is ProductCategory.None)
            .ThenBy(x => x.Key)
            .SelectMany(x =>
            {
                var result = new List<InlineKeyboardButton[]>
                {
                    InlineKeyboardButton
                        .WithCallbackData(ProductCategories.ProductCategoryDisplayNameMap[x.Key])
                        .ToOneElementArray(),
                };

                result.AddRange(x
                    .Select(y => InlineKeyboardButton
                        .WithCallbackData(
                            Messages.ProductButtonText(y),
                            JsonConvert.SerializeObject(new MarkBoughtProductDto
                            {
                                Id = y.Id,
                                Command = Commands.ToggleBuyProductQueryCommand
                            }))
                        .ToOneElementArray())
                    .ToArray());

                return result;
            })
            .ToArray();

        return new InlineKeyboardMarkup(productByCategoriesButtons);
    }
}
