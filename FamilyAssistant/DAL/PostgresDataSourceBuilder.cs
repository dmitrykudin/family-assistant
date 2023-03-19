using FamilyAssistant.DAL.Types;
using Npgsql;
using Npgsql.NameTranslation;

namespace FamilyAssistant.DAL;

public static class PostgresDataSourceBuilder
{
    public static NpgsqlDataSource Build(string connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var translator = new NpgsqlSnakeCaseNameTranslator();

        dataSourceBuilder.MapComposite<ProductToBuyV1>("product_to_buy_v1", translator);
        dataSourceBuilder.MapComposite<ProductV1>("product_v1", translator);

        return dataSourceBuilder.Build();
    }
}
