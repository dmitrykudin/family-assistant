using System.Text.RegularExpressions;
using FamilyAssistant.Constants;
using FamilyAssistant.Enums;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.Services;
using FamilyAssistant.Models;
using FuzzySharp;
using FuzzySharp.SimilarityRatio.Scorer.Composite;
using Newtonsoft.Json;

namespace FamilyAssistant.Services;

public class ProductCategoryService : IProductCategoryService
{
    private const string ProductCategoriesFileName = "productCategories.json";

    private readonly Regex _preprocess = new("[^ a-zA-Zа-яА-я0-9]", RegexOptions.Compiled);
    private readonly Dictionary<ProductCategory, string[]> _productCategoryTokens;

    public ProductCategoryService()
    {
        var productCategories = GetProductCategories();

        _productCategoryTokens = new Dictionary<ProductCategory, string[]>(productCategories
            ?.Where(x => ProductCategories.ProductCategoryNameMap.ContainsValue(x.Key))
            .Select(x =>
            {
                var category = ProductCategories.ProductCategoryNameMap
                    .FirstOrDefault(y => y.Value == x.Key);

                return new KeyValuePair<ProductCategory, string[]>(category.Key, x.Value);
            }) ?? Array.Empty<KeyValuePair<ProductCategory, string[]>>());
    }

    public Dictionary<string, ProductCategory> MapProductCategory(string[] products)
    {
        var mapResults = products
            .AsParallel()
            .Select(x => new ProductCategoryMapResult
            {
                Product = x,
                CategoryResults = _productCategoryTokens
                    .AsParallel()
                    .Select(y => new ProductCategoryMapResult.CategoryResult
                    {
                        Category = y.Key,
                        Results = Process.ExtractTop(
                                x,
                                y.Value,
                                processor: s => _preprocess
                                    .Replace(s, " ")
                                    .ToLowerInvariant()
                                    .Trim(),
                                scorer: new WeightedRatioScorer(),
                                limit: 3,
                                cutoff: 70)
                            .ToArray(),
                    })
                    .Where(y => !y.Results.IsNullOrEmpty())
                    .ToArray(),
            })
            .ToArray();

        return mapResults.ToDictionary(
            x => x.Product,
            x => !x.CategoryResults.IsNullOrEmpty()
                ? x.CategoryResults
                    .MaxBy(y => y.Results.Sum(z => z.Score) / y.Results.Length)
                    !.Category
                : ProductCategory.None);
    }

    private static Dictionary<string, string[]>? GetProductCategories()
    {
        var productCategoriesFilePath = Path.Combine(Directory.GetCurrentDirectory(), ProductCategoriesFileName);

        using var r = new StreamReader(productCategoriesFilePath);

        var json = r.ReadToEnd();

        return JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
    }
}
