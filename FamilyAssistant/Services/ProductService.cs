using FamilyAssistant.DAL.Models;
using FamilyAssistant.DAL.Types;
using FamilyAssistant.Enums;
using FamilyAssistant.Interfaces.DAL.Repositories;
using FamilyAssistant.Interfaces.Services;
using FamilyAssistant.Models;

namespace FamilyAssistant.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    private readonly IProductCategoryService _productCategoryService;

    public ProductService(IProductRepository productRepository, IProductCategoryService productCategoryService)
    {
        _productRepository = productRepository;
        _productCategoryService = productCategoryService;
    }

    public async Task<ProductDto[]> GetOrCreate(string[] products, CancellationToken token)
    {
        var existingProducts = await _productRepository.Query(
            new QueryProductsModel
            {
                Names = products,
            },
            token);

        var newProducts = products
            .Except(existingProducts.Select(x => x.Name))
            .ToArray();

        var productToCategoryMap = _productCategoryService.MapProductCategory(newProducts);

        var productsToCreate = newProducts
            .Select(x => new ProductV2
            {
                Name = x,
                ProductCategoryId = (int)productToCategoryMap[x],
            })
            .ToArray();

        var createdProducts = await _productRepository.Insert(productsToCreate, token);

        return existingProducts
            .Concat(createdProducts)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Category = (ProductCategory)x.ProductCategoryId,
            })
            .ToArray();
    }

    public async Task<ProductDto[]> GetById(long[] ids, CancellationToken token)
    {
        var products = await _productRepository.Query(new QueryProductsModel { Ids = ids, }, token);

        return products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Category = (ProductCategory)x.ProductCategoryId,
            })
            .ToArray();
    }
}
