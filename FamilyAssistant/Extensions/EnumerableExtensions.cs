namespace FamilyAssistant.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => source == null || !source.Any();

    public static IEnumerable<IEnumerable<T>?> Batch<T>(this IEnumerable<T>? source, int size)
    {
        if (source.IsNullOrEmpty())
        {
            yield return null;
        }

        var length = source!.Count();
        var position = 0;
        do
        {
            yield return source!.Skip(position).Take(size);
            position += size;
        }
        while (position < length);
    }

    public static T[] ToOneElementArray<T>(this T item)
        where T : class
        => new[] { item };
}
