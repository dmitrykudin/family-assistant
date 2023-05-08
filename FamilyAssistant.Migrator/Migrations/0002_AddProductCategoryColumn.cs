using FluentMigrator;

namespace FamilyAssistant.Migrator.Migrations;

[Migration(2)]
public class AddProductCategoryColumn : Migration
{
    public override void Up()
    {
        const string sql = @"
            ALTER TABLE IF EXISTS product ADD COLUMN product_category INTEGER NOT NULL DEFAULT 0;

            DO
            $$
                BEGIN
                    IF NOT EXISTS (SELECT * FROM pg_type WHERE typname = 'product_v2') THEN
                        CREATE TYPE product_v2 AS (
                                id                BIGINT,
                                name              TEXT,
                                created_at        TIMESTAMP WITH TIME ZONE,
                                product_category  INTEGER
                            );
                    END IF;
                END
            $$;
        ";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        const string sql = @"
            ALTER TABLE IF EXISTS product DROP COLUMN product_category;

            DROP TYPE IF EXISTS product_v2;
        ";

        Execute.Sql(sql);
    }
}
