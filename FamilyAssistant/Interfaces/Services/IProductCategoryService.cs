using FamilyAssistant.Enums;

namespace FamilyAssistant.Interfaces.Services;

public interface IProductCategoryService
{
    Dictionary<string, ProductCategory> MapProductCategory(string[] products);
}
