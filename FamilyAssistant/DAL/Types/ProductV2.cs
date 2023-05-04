namespace FamilyAssistant.DAL.Types;

public class ProductV2
{
    public long Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }

    public int ProductCategoryId { get; set; }
}
