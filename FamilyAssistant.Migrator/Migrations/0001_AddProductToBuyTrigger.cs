using FluentMigrator;

namespace FamilyAssistant.Migrator.Migrations;

[Migration(1)]
public class AddProductToBuyTrigger : Migration
{
    public override void Up()
    {
        var sql = @"
            CREATE OR REPLACE FUNCTION log_product_to_buy_changes()
                RETURNS TRIGGER AS
            $BODY$
            DECLARE
                action_value CHAR;
                now_value    TIMESTAMP WITH TIME ZONE = now();
            BEGIN
                IF (tg_op = 'INSERT') THEN
                    NEW.created_at = now_value;
                    NEW.updated_at = now_value;
                    action_value = 'I';
                ELSEIF (tg_op = 'UPDATE') THEN
                    NEW.updated_at = now_value;
                    action_value = 'U';
                ELSEIF (tg_op = 'DELETE') THEN
                    INSERT INTO product_to_buy_log (
                        product_to_buy_id,
                        product_id,
                        name,
                        quantity,
                        created_at,
                        updated_at,
                        is_bought,
                        ""action""
                    ) VALUES (
                        old.id,
                        old.product_id,
                        old.name,
                        old.quantity,
                        old.created_at,
                        now_value,
                        old.is_bought
                        'D'
                    );

                    RETURN OLD;
                END IF;

                INSERT INTO product_to_buy_log (
                    product_to_buy_id,
                    product_id,
                    name,
                    quantity,
                    created_at,
                    updated_at,
                    is_bought,
                    ""action""
                ) VALUES (
                    new.id,
                    new.product_id,
                    new.name,
                    new.quantity,
                    new.created_at,
                    new.updated_at,
                    new.is_bought,
                    action_value
                );

                RETURN NEW;
            END
            $BODY$
                LANGUAGE 'plpgsql' VOLATILE;

            CREATE TRIGGER product_to_buy_log_trigger
                BEFORE INSERT OR UPDATE OR DELETE
                ON product_to_buy
                FOR EACH ROW
            EXECUTE PROCEDURE log_product_to_buy_changes();
        ";

        Execute.Sql(sql);
    }

    public override void Down()
    {
        var sql = @"
            DROP TRIGGER IF EXISTS product_to_buy_log_trigger ON product_to_buy;

            DROP FUNCTION IF EXISTS log_product_to_buy_changes;
        ";

        Execute.Sql(sql);
    }
}
