namespace FamilyAssistant.Constants;

public static class Commands
{
    public const string BuyProductCommand = "/buy";
    public const string GetProductsToBuyCommand = "/get_to_buy";

    public const string ToggleBuyProductQueryCommand = "/toggleBuy";

    public static readonly Dictionary<string, string> CommandToDescriptionMap = new()
    {
        { BuyProductCommand, "Купить продукты" },
        { GetProductsToBuyCommand, "Что нужно купить?" },
    };
}
