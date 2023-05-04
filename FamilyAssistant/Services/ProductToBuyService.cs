using FamilyAssistant.DAL.Models;
using FamilyAssistant.DAL.Types;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.DAL.Repositories;
using FamilyAssistant.Interfaces.Services;
using FamilyAssistant.Models;

namespace FamilyAssistant.Services;

public class ProductToBuyService : IProductToBuyService
{
    private readonly IProductToBuyRepository _productToBuyRepository;

    private readonly IProductService _productService;

    public ProductToBuyService(IProductToBuyRepository productToBuyRepository, IProductService productService)
    {
        _productToBuyRepository = productToBuyRepository;

        _productService = productService;
    }

    public async Task AddProductsToBuy(AddProductToBuyDto[] productsToBuy, CancellationToken token)
    {
        var names = productsToBuy.Select(x => x.Name).ToArray();
        var productsToBuyDictionary = productsToBuy
            .DistinctBy(x => x.Name)
            .ToDictionary(x => x.Name, x => x.Quantity);

        var products = (await _productService.GetOrCreate(names, token))
            .ToDictionary(x => x.Name, x => x.Id);
        var existingProductsToBuy = (await _productToBuyRepository.Query(
                new QueryProductsToBuyModel
                {
                    ProductIds = products.Values.ToArray(),
                    Bought = false,
                },
                token))
            .ToDictionary(x => x.Name);

        var newProductsToBuy = productsToBuyDictionary
            .Where(x => !existingProductsToBuy.ContainsKey(x.Key))
            .ToArray();
        if (!newProductsToBuy.IsNullOrEmpty())
        {
            await _productToBuyRepository.Create(
                newProductsToBuy
                    .Select(x => new ProductToBuyV1
                    {
                        ProductId = products[x.Key],
                        Name = x.Key,
                        Quantity = x.Value,
                        CreatedAt = DateTimeOffset.Now.ToUniversalTime(),
                        UpdatedAt = DateTimeOffset.Now.ToUniversalTime(),
                    })
                    .ToArray(),
                token);
        }

        var updateProductsToBuy = new List<ProductToBuyV1>();
        foreach (var existingProductToBuy in existingProductsToBuy)
        {
            if (productsToBuyDictionary.TryGetValue(existingProductToBuy.Key, out var newQuantity)
                && !newQuantity.IsNullOrEmpty())
            {
                existingProductToBuy.Value.Quantity = newQuantity;
                updateProductsToBuy.Add(existingProductToBuy.Value);
            }
        }

        if (!updateProductsToBuy.IsNullOrEmpty())
        {
            await _productToBuyRepository.UpdateQuantity(updateProductsToBuy.ToArray(), token);
        }
    }

    public async Task<ProductToBuyDto[]> GetProductsToBuy(CancellationToken token)
    {
        var hourAgo = DateTimeOffset.UtcNow.AddHours(-1);
        var productsToBuy = await _productToBuyRepository.Query(
            new QueryProductsToBuyModel
            {
                Bought = false,
                UpdatedAtFrom = new ConditionModel<DateTimeOffset>(hourAgo, ConditionType.OR),
            },
            token);

        var products = (await _productService.GetById(
                productsToBuy
                    .Select(x => x.ProductId)
                    .Distinct()
                    .ToArray(),
                token))
            .ToDictionary(x => x.Id);

        return productsToBuy
            .GroupBy(x => x.Name)
            .Select(x => x.Count() > 1
                ? x.FirstOrDefault(y => !y.IsBought)
                : x.FirstOrDefault())
            .Where(x => x is not null)
            .Select(x => new ProductToBuyDto
            {
                Id = x!.Id,
                ProductId = x.ProductId,
                Product = products.TryGetValue(x.ProductId, out var product)
                    ? product
                    : null,
                Name = x.Name,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                IsBought = x.IsBought,
            })
            .OrderBy(x => x.Name)
            .ToArray();
    }

    public async Task<ProductToBuyDto?> GetById(long id, CancellationToken token)
    {
        var products = await _productToBuyRepository.Query(
            new QueryProductsToBuyModel
            {
                Ids = new[] { id },
            },
            token);

        var product = products.FirstOrDefault();
        return product == null
            ? null
            : new ProductToBuyDto
            {
                Id = product.Id,
                ProductId = product.ProductId,
                Name = product.Name,
                Quantity = product.Quantity,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                IsBought = product.IsBought,
            };
    }

    public async Task MarkProductBought(long id, CancellationToken token) =>
        await _productToBuyRepository.MarkAsBought(new[] { id }, token);

    public async Task MarkProductToBuy(long id, CancellationToken token) =>
        await _productToBuyRepository.MarkAsToBuy(new[] { id }, token);
}
