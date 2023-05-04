using FamilyAssistant.DAL.Models;
using FamilyAssistant.DAL.Types;

namespace FamilyAssistant.Interfaces.DAL.Repositories;

public interface IProductRepository
{
    Task<ProductV2[]> Insert(ProductV2[] products, CancellationToken token);

    Task<ProductV2[]> Query(QueryProductsModel query, CancellationToken token);

    Task<ProductV2[]> GetByNamesOrCreate(ProductV2[] products, CancellationToken token);

    Task<ProductV2[]> GetByIds(long[] ids, CancellationToken token);
}
