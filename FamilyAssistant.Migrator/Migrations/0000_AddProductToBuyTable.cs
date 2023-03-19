using FluentMigrator;

namespace FamilyAssistant.Migrator.Migrations;

[Migration(0)]
public class AddProductToBuyTable : Migration
{
    public override void Up()
    {
        var sql = @"
            CREATE TABLE IF NOT EXISTS product (
                id          BIGSERIAL PRIMARY KEY,
                name        TEXT NOT NULL,
                created_at  TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now()
            );

            CREATE TABLE IF NOT EXISTS product_to_buy (
                id          BIGSERIAL PRIMARY KEY,
                product_id  BIGINT NOT NULL,
                name        TEXT NOT NULL,
                quantity    VARCHAR(255),
                created_at  TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
                updated_at  TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
                is_bought   BOOL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS product_to_buy_log (
                id                  BIGSERIAL PRIMARY KEY,
                product_to_buy_id   BIGINT NOT NULL,
                product_id          BIGINT NOT NULL,
                name                TEXT NOT NULL,
                quantity            VARCHAR(255),
                created_at          TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
                updated_at          TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT now(),
                is_bought           BOOL NOT NULL,
                ""action""          CHAR
            );

            DO
            $$
                BEGIN
                    IF NOT EXISTS (SELECT * FROM pg_type WHERE typname = 'product_v1') THEN
                        CREATE TYPE product_v1 AS (
                                id          BIGINT,
                                name        TEXT,
                                created_at  TIMESTAMP WITH TIME ZONE
                            );
                    END IF;
                END
            $$;

            DO
            $$
                BEGIN
                    IF NOT EXISTS (SELECT * FROM pg_type WHERE typname = 'product_to_buy_v1') THEN
                        CREATE TYPE product_to_buy_v1 AS (
                                id          BIGINT,
                                product_id  BIGINT,
                                name        TEXT,
                                quantity    VARCHAR(255),
                                created_at  TIMESTAMP WITH TIME ZONE,
                                updated_at  TIMESTAMP WITH TIME ZONE,
                                is_bought   BOOL
                            );
                    END IF;
                END
            $$;
        ";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        var sql = @"
            DROP TYPE IF EXISTS product_v1;
            DROP TYPE IF EXISTS product_to_buy_v1;

            DROP TABLE IF EXISTS product;
            DROP TABLE IF EXISTS product_to_buy;
            DROP TABLE IF EXISTS product_to_buy_log;
        ";

        Execute.Sql(sql);
    }
}
