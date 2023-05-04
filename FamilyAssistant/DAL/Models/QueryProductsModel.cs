namespace FamilyAssistant.DAL.Models;

public class QueryProductsModel
{
    public ConditionModel<long[]>? Ids { get; set; }

    public ConditionModel<string[]>? Names { get; set; }

    public ConditionModel<int[]>? ProductCategories { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }
}
