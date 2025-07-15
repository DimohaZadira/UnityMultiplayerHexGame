using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerable<U> Map<T, U>(this IEnumerable<T> s, System.Func<T, U> f)
    {
        foreach (var item in s)
            yield return f(item);
    }
}
