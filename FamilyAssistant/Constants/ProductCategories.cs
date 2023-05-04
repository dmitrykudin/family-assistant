using System.Diagnostics.CodeAnalysis;
using FamilyAssistant.Enums;

namespace FamilyAssistant.Constants;

[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "This is just Russian.")]
public static class ProductCategories
{
    public static Dictionary<ProductCategory, string> ProductCategoryNameMap = new()
    {
        { ProductCategory.None, "🤷‍♀️ Другое" },
        { ProductCategory.Fruits, "🍉 Фрукты" },
        { ProductCategory.Vegetables, "🥕 Овощи" },
        { ProductCategory.Seafood, "🐟 Морепродукты" },
        { ProductCategory.Dairy, "🥛 Молочные продукты" },
        { ProductCategory.Grocery, "🍚 Бакалея" },
        { ProductCategory.Drinks, "🥤 Напитки" },
        { ProductCategory.Snakcs, "🍔 Снеки" },
        { ProductCategory.CannedFood, "🥫 Консервы" },
        { ProductCategory.FrozenFood, "🧊 Замороженные продукты" },
        { ProductCategory.HouseholdChemicals, "🧴 Бытовая химия" },
        { ProductCategory.PersonalHygeine, "🪥 Личная гигиена" },
        { ProductCategory.ForHome, "🏠 Для дома" },
        { ProductCategory.Health, "💊 Здоровье" },
        { ProductCategory.Appliances, "🎛️ Бытовая техника" },
        { ProductCategory.Sweets, "🍫 Сладости" },
        { ProductCategory.Bakery, "🥐 Выпечка" },
    };
}
