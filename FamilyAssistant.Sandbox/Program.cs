// See https://aka.ms/new-console-template for more information

using FamilyAssistant.Services;

var productCategoryService = new ProductCategoryService();

var categories = productCategoryService.MapProductCategory(new[]
{
    "Мороженое",
});

foreach (var category in categories)
{
    Console.WriteLine(category.Key + " " + category.Value);
}
