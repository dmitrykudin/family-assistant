using FamilyAssistant.Models;

namespace FamilyAssistant.Constants;

public static class Messages
{
    public const string SorryICantDoThisYet = "😬 Извините, я пока что не умею такое.";

    public const string TypeWhatYouWantToBuy = "🙄 Напишите, что нужно купить?";

    public const string MarkBoughtProducts = "✅ Отметьте ниже продукты, которые вы купили.";

    public const string YouBoughtAllProducts = "👍 Отлично, вы купили все продукты!";

    public const string YouHaveNoProductsToBuy = "👐 У вас нет продуктов, которые нужно купить.";

    public const string HereAreYourProductsToBuy = "🛒 Вот продукты, которые нужно купить:";

    public static readonly Func<string, string> MarkedThatINeedToBuyThis =
        product => $"✅ Отметил что нужно купить {product}.";

    public static readonly Func<string, string> MarkedThatIBoughtThis =
        product => $"✅ Отметил что продукт {product} куплен";

    public static readonly Func<ProductToBuyDto, string> ProductListItem = x =>
    {
        var text = ProductButtonText(x);

        return x.IsBought ? Strikethrough(text) : text;
    };

    public static readonly Func<ProductToBuyDto, string> ProductButtonText =
        x => (x.IsBought ? "⚫ " : "🔴 ") + ProductText(x);

    public static readonly Func<ProductToBuyDto, string> ProductText =
        x => x.Name + (!string.IsNullOrEmpty(x.Quantity) ? $" {x.Quantity}" : string.Empty);

    public static readonly Func<AddProductToBuyDto, string> AddProductText =
        x => x.Name + (!string.IsNullOrEmpty(x.Quantity) ? $" {x.Quantity}" : string.Empty);

    private static readonly Func<string, string> Strikethrough = x => $"<s>{x}</s>";

    private static readonly Func<string, string> Bold = x => $"<bold>{x}</bold>";
}
