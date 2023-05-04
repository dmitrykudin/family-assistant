using FamilyAssistant.Enums;

namespace FamilyAssistant.Models;

public class ProductDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public ProductCategory Category { get; set; }
}
