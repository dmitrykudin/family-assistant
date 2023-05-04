using FamilyAssistant.Models;

namespace FamilyAssistant.Interfaces.Services;

public interface IProductService
{
    Task<ProductDto[]> GetOrCreate(string[] products, CancellationToken token);

    Task<ProductDto[]> GetById(long[] ids, CancellationToken token);
}
