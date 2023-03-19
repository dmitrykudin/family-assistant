namespace FamilyAssistant.DAL.Models;

public class QueryProductsToBuyModel
{
    public ConditionModel<long[]>? Ids { get; set; }

    public ConditionModel<long[]>? ProductIds { get; set; }

    public ConditionModel<string[]>? Names { get; set; }

    public ConditionModel<bool>? Bought { get; set; }

    public ConditionModel<DateTimeOffset>? UpdatedAtFrom { get; set; }

    public ConditionModel<DateTimeOffset>? UpdatedAtTo { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }
}
