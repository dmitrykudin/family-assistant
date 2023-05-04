using FamilyAssistant.Enums;
using FuzzySharp.Extractor;

namespace FamilyAssistant.Models;

public class ProductCategoryMapResult
{
    public string Product { get; set; }

    public CategoryResult[] CategoryResults { get; set; }

    public class CategoryResult
    {
        public ProductCategory Category { get; set; }

        public ExtractedResult<string>[] Results { get; set; }
    }
}
