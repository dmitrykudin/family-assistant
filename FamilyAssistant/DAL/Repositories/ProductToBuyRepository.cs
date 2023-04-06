using Dapper;
using FamilyAssistant.DAL.Models;
using FamilyAssistant.DAL.Types;
using FamilyAssistant.Extensions;
using FamilyAssistant.Interfaces.DAL.Repositories;
using Npgsql;

namespace FamilyAssistant.DAL.Repositories;

public class ProductToBuyRepository : IProductToBuyRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ProductToBuyRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task Create(ProductToBuyV1[] products, CancellationToken token)
    {
        var sql = @"
            INSERT INTO product_to_buy (
                product_id,
                name,
                quantity,
                created_at,
                updated_at,
                is_bought)
            SELECT product_id,
                   name,
                   quantity,
                   created_at,
                   updated_at,
                   is_bought
            FROM unnest(@Products)";

        var command = new CommandDefinition(sql, parameters: new { Products = products });

        var connection = await _dataSource.OpenConnectionAsync(token);
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateQuantity(ProductToBuyV1[] products, CancellationToken token)
    {
        var sql = @"
            UPDATE product_to_buy p1
            SET quantity = p2.quantity,
                updated_at = now()
            FROM unnest(@Products) as p2
            WHERE p1.id = p2.id";

        var command = new CommandDefinition(sql, parameters: new { Products = products });

        var connection = await _dataSource.OpenConnectionAsync(token);
        await connection.ExecuteAsync(command);
    }

    public async Task MarkAsBought(long[] productIds, CancellationToken token) =>
        await SetIsBought(true, productIds, token);

    public async Task MarkAsToBuy(long[] productIds, CancellationToken token) =>
        await SetIsBought(false, productIds, token);

    public async Task<ProductToBuyV1[]> Query(QueryProductsToBuyModel query, CancellationToken token)
    {
        var sql = @"
            SELECT
                id,
                product_id,
                name,
                quantity,
                created_at,
                updated_at,
                is_bought
            FROM product_to_buy
        ";

        var parameters = new DynamicParameters();
        var conditions = new List<(ConditionType, string)>();

        if (!query.Ids?.Value.IsNullOrEmpty() ?? false)
        {
            const string idsParameterName = nameof(query.Ids);
            parameters.Add(idsParameterName, query.Ids.Value);
            conditions.Add((query.Ids.Type, $"id = ANY(@{idsParameterName})"));
        }

        if (!query.ProductIds?.Value.IsNullOrEmpty() ?? false)
        {
            const string productIdsParameterName = nameof(query.ProductIds);
            parameters.Add(productIdsParameterName, query.ProductIds.Value);
            conditions.Add((query.ProductIds.Type, $"product_id = ANY(@{productIdsParameterName})"));
        }

        if (!query.Names?.Value.IsNullOrEmpty() ?? false)
        {
            const string namesParameterName = nameof(query.Names);
            parameters.Add(namesParameterName, query.Names.Value);
            conditions.Add((query.Names.Type, $"name = ANY(@{namesParameterName})"));
        }

        if (query.Bought is not null)
        {
            const string boughtParameterName = nameof(query.Bought);
            parameters.Add(boughtParameterName, query.Bought.Value);
            conditions.Add((query.Bought.Type, $"is_bought = @{boughtParameterName}"));
        }

        if (query.UpdatedAtFrom is not null)
        {
            const string updatedAtFromParameterName = nameof(query.UpdatedAtFrom);
            parameters.Add(updatedAtFromParameterName, query.UpdatedAtFrom.Value);
            conditions.Add((query.UpdatedAtFrom.Type, $"updated_at >= @{updatedAtFromParameterName}"));
        }

        if (query.UpdatedAtTo is not null)
        {
            const string updatedAtToParameterName = nameof(query.UpdatedAtTo);
            parameters.Add(updatedAtToParameterName, query.UpdatedAtTo.Value);
            conditions.Add((query.UpdatedAtTo.Type, $"updated_at < @{updatedAtToParameterName}"));
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

        return (await connection.QueryAsync<ProductToBuyV1>(command)).ToArray();
    }

    private async Task SetIsBought(bool isBought, long[] productToBuyIds, CancellationToken token)
    {
        var sql = @"
            UPDATE product_to_buy
            SET is_bought = @IsBought,
                updated_at = now()
            WHERE id = ANY(@ProductToBuyIds)";

        var command = new CommandDefinition(sql, parameters: new
        {
            ProductToBuyIds = productToBuyIds,
            IsBought = isBought,
        });

        var connection = await _dataSource.OpenConnectionAsync(token);
        await connection.ExecuteAsync(command);
    }
}
