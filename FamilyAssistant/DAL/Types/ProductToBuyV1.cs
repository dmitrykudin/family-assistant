namespace FamilyAssistant.DAL.Types;

public class ProductToBuyV1
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Name { get; set; }

    public string Quantity { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public bool IsBought { get; set; }
}
