using Dapper;
using FamilyAssistant.DAL.Types;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.DAL.Repositories;
using Npgsql;

namespace FamilyAssistant.DAL.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ProductRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<ProductV1[]> GetOrCreate(string[] names, CancellationToken token)
    {
        var productsFromDb = (await SelectByNames(names, token))
            .ToDictionary(x => x.Name);

        var productsToCreate = names.Except(productsFromDb.Keys).ToArray();
        if (productsToCreate.IsNullOrEmpty())
        {
            return productsFromDb.Values.ToArray();
        }

        await Insert(productsToCreate, token);
        var createdProducts = await SelectByNames(productsToCreate, token);

        return productsFromDb.Values.ToArray()
            .Concat(createdProducts)
            .ToArray();

    }

    private async Task<ProductV1[]> SelectByNames(string[] names, CancellationToken token)
    {
        var sql = @"
            SELECT
                id,
                name,
                created_at
            FROM product
            WHERE name = ANY(@Names)";

        var command = new CommandDefinition(sql, parameters: new { Names = names });
        var connection = await _dataSource.OpenConnectionAsync(token);

        return (await connection.QueryAsync<ProductV1>(command)).ToArray();
    }

    private async Task Insert(string[] names, CancellationToken token)
    {
        var sql = @"
            INSERT INTO product (name)
            SELECT * FROM unnest(@Names)";

        var command = new CommandDefinition(sql, parameters: new { Names = names });

        var connection = await _dataSource.OpenConnectionAsync(token);
        await connection.ExecuteAsync(command);
    }
}
