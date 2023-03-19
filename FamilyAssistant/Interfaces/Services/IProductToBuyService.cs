using FamilyAssistant.Models;

namespace FamilyAssistant.Interfaces.Services;

public interface IProductToBuyService
{
    Task AddProductsToBuy(AddProductToBuyDto[] productsToBuy, CancellationToken token);

    Task<ProductToBuyDto[]> GetProductsToBuy(CancellationToken token);

    Task<ProductToBuyDto?> GetById(long id, CancellationToken token);

    Task MarkProductBought(long id, CancellationToken token);

    Task MarkProductToBuy(long id, CancellationToken token);
}
