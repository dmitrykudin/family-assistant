using System.Diagnostics.CodeAnalysis;
using FamilyAssistant.Enums;

namespace FamilyAssistant.Constants;

[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "This is just Russian.")]
public static class ProductCategories
{
    public static readonly Dictionary<ProductCategory, string> ProductCategoryNameMap = new()
    {
        { ProductCategory.None, "Другое" },
        { ProductCategory.Fruits, "Фрукты" },
        { ProductCategory.Vegetables, "Овощи" },
        { ProductCategory.Seafood, "Морепродукты" },
        { ProductCategory.Dairy, "Молочные продукты" },
        { ProductCategory.Grocery, "Бакалея" },
        { ProductCategory.Drinks, "Напитки" },
        { ProductCategory.Snacks, "Снеки" },
        { ProductCategory.CannedFood, "Консервы" },
        { ProductCategory.FrozenFood, "Замороженные продукты" },
        { ProductCategory.HouseholdChemicals, "Бытовая химия" },
        { ProductCategory.PersonalHygiene, "Личная гигиена" },
        { ProductCategory.ForHome, "Для дома" },
        { ProductCategory.Health, "Здоровье" },
        { ProductCategory.Appliances, "Бытовая техника" },
        { ProductCategory.Sweets, "Сладости" },
        { ProductCategory.Bakery, "Выпечка" },
        { ProductCategory.Pets, "Для животных" }
    };

    public static readonly Dictionary<ProductCategory, string> ProductCategoryDisplayNameMap = new()
    {
        { ProductCategory.None, "🤷‍♀️ Другое" },
        { ProductCategory.Fruits, "🍉 Фрукты" },
        { ProductCategory.Vegetables, "🥕 Овощи" },
        { ProductCategory.Seafood, "🐟 Морепродукты" },
        { ProductCategory.Dairy, "🥛 Молочные продукты" },
        { ProductCategory.Grocery, "🍚 Бакалея" },
        { ProductCategory.Drinks, "🥤 Напитки" },
        { ProductCategory.Snacks, "🍔 Снеки" },
        { ProductCategory.CannedFood, "🥫 Консервы" },
        { ProductCategory.FrozenFood, "🧊 Замороженные продукты" },
        { ProductCategory.HouseholdChemicals, "🧴 Бытовая химия" },
        { ProductCategory.PersonalHygiene, "🪥 Личная гигиена" },
        { ProductCategory.ForHome, "🏠 Для дома" },
        { ProductCategory.Health, "💊 Здоровье" },
        { ProductCategory.Appliances, "🎛️ Бытовая техника" },
        { ProductCategory.Sweets, "🍫 Сладости" },
        { ProductCategory.Bakery, "🥐 Выпечка" },
        { ProductCategory.Pets, "🐱 Для животных" }
    };
}
