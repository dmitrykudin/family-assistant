using FamilyAssistant.DAL.Types;

namespace FamilyAssistant.Interfaces.DAL.Repositories;

public interface IProductRepository
{
    Task<ProductV1[]> GetOrCreate(string[] names, CancellationToken token);
}
