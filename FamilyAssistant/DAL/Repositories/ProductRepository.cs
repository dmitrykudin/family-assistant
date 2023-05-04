using Dapper;
using FamilyAssistant.DAL.Models;
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

    public async Task<ProductV2[]> GetByNamesOrCreate(ProductV2[] products, CancellationToken token)
    {
        var productNames = products.Select(x => x.Name).ToArray();
        var productsFromDb = (await SelectByNames(productNames, token))
            .ToDictionary(x => x.Name);

        var productsToCreate = products
            .ExceptBy(productsFromDb.Keys, x => x.Name)
            .ToArray();
        if (productsToCreate.IsNullOrEmpty())
        {
            return productsFromDb.Values.ToArray();
        }

        var createdProducts = await Insert(productsToCreate, token);

        return productsFromDb.Values.ToArray()
            .Concat(createdProducts)
            .ToArray();

    }

    public async Task<ProductV2[]> GetByIds(long[] ids, CancellationToken token)
        => await Query(new QueryProductsModel { Ids = ids }, token);

    private async Task<ProductV2[]> SelectByNames(string[] names, CancellationToken token)
        => await Query(new QueryProductsModel { Names = names }, token);

    public async Task<ProductV2[]> Query(QueryProductsModel query, CancellationToken token)
    {
        var sql = @"
            SELECT
                id,
                name,
                created_at,
                product_category
            FROM product
        ";

        var parameters = new DynamicParameters();
        var conditions = new List<(ConditionType, string)>();

        if (!query.Ids?.Value.IsNullOrEmpty() ?? false)
        {
            const string idsParameterName = nameof(query.Ids);
            parameters.Add(idsParameterName, query.Ids.Value);
            conditions.Add((query.Ids.Type, $"id = ANY(@{idsParameterName})"));
        }

        if (!query.Names?.Value.IsNullOrEmpty() ?? false)
        {
            const string namesParameterName = nameof(query.Names);
            parameters.Add(namesParameterName, query.Names.Value);
            conditions.Add((query.Names.Type, $"name = ANY(@{namesParameterName})"));
        }

        if (!query.ProductCategories?.Value.IsNullOrEmpty() ?? false)
        {
            const string productCategoriesParameterName = nameof(query.ProductCategories);
            parameters.Add(productCategoriesParameterName, query.ProductCategories.Value);
            conditions.Add((query.ProductCategories.Type, $"product_category = ANY(@{productCategoriesParameterName})"));
        }

        if (conditions.Count > 0)
        {
            sql += $" WHERE {string.Join(' ', conditions
                .Select((x, index)
                    => (index != 0 ? $"{x.Item1.ToString()} " : string.Empty) + x.Item2))}";
        }

        if (query.Limit.HasValue)
        {
            sql += $" LIMIT {query.Limit.Value}";
        }

        if (query.Offset.HasValue)
        {
            sql += $" OFFSET {query.Offset.Value}";
        }

        var command = new CommandDefinition(sql, parameters);
        var connection = await _dataSource.OpenConnectionAsync(token);

        return (await connection.QueryAsync<ProductV2>(command)).ToArray();
    }

    public async Task<ProductV2[]> Insert(ProductV2[] products, CancellationToken token)
    {
        var sql = @"
            INSERT INTO product (
                name,
                product_category)
            SELECT
                name,
                product_category
            FROM unnest(@Products)
            RETURNING
                product.id,
                product.name,
                product.created_at,
                product.product_category;";

        var command = new CommandDefinition(sql, parameters: new { Products = products });
        var connection = await _dataSource.OpenConnectionAsync(token);

        return (await connection.QueryAsync<ProductV2>(command)).ToArray();
    }
}
