namespace FamilyAssistant.DAL.Models;

public class ConditionModel<T>
{
    public ConditionModel(T value, ConditionType type = ConditionType.AND)
    {
        Value = value;
        Type = type;
    }

    public T Value { get; set; }

    public ConditionType Type { get; set; }

    public static implicit operator ConditionModel<T>(T value) => new(value);
}
