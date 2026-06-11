using System.Collections.Generic;

public static class ListExtensions
{
    public static T GetRandom<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
            return default;

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static List<T> GetRandomElements<T>(this IList<T> list, int count)
    {
        if (list == null || list.Count == 0) return new List<T>();

        List<T> copy = new List<T>(list);
        copy.Shuffle();

        int resultCount = UnityEngine.Mathf.Min(count, copy.Count);
        return copy.GetRange(0, resultCount);
    }
}
