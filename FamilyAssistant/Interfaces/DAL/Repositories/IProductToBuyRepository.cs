using FamilyAssistant.DAL.Models;
using FamilyAssistant.DAL.Types;

namespace FamilyAssistant.Interfaces.DAL.Repositories;

public interface IProductToBuyRepository
{
    Task Create(ProductToBuyV1[] products, CancellationToken token);

    Task MarkAsBought(long[] productIds, CancellationToken token);

    Task MarkAsToBuy(long[] productIds, CancellationToken token);

    Task<ProductToBuyV1[]> Query(QueryProductsToBuyModel query, CancellationToken token);
}
