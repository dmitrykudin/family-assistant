namespace FamilyAssistant.DAL.Types;

public class ProductV1
{
    public long Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
}
