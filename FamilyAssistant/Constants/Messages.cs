using FamilyAssistant.Models;

namespace FamilyAssistant.Constants;

public static class Messages
{
    public const string SorryICantDoThisYet = "😬 Извините, я пока что не умею такое.";

    public const string TypeWhatYouWantToBuy = "🙄 Напишите, что нужно купить?";

    public const string MarkBoughtProducts = "✅ Отметьте ниже продукты, которые вы купили.";

    public const string YouBoughtAllProducts = "👍 Отлично, вы купили все продукты!";

    public const string YouHaveNoProductsToBuy = "👐 У вас нет продуктов, которые нужно купить.";

    public static readonly Func<string, string> MarkedThatINeedToBuyThis =
        product => $"✅ Отметил что нужно купить {product}.";

    public static readonly Func<string, string> MarkedThatIBoughtThis =
        product => $"✅ Отметил что продукт {product} куплен";

    public static readonly Func<ProductToBuyDto[], string> HereAreYourProductsToBuy =
        products =>
        {
            var message = "🛍 Вот продукты, которые нужно купить:\n\n";

            var productLines = products
                .Select(x => ProductListItem(x));

            message += $"{string.Join("\n", productLines)}\n\n";
            message += $"Отметить купленные продукты можно кнопками ниже.";

            return message;
        };

    public static readonly Func<ProductToBuyDto, string> ProductListItem = x =>
    {
        var text = ProductButtonText(x);

        return x.IsBought ? Strikethrough(text) : text;
    };

    public static readonly Func<ProductToBuyDto, string> ProductButtonText =
        x => (x.IsBought ? "✔" : "❗") + $" {x.Name} {x.Quantity}";

    private static readonly Func<string, string> Strikethrough = x => $"<s>{x}</s>";
}
