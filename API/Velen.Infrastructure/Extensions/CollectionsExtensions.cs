namespace Velen.Infrastructure.Extensions;

/// <summary>
/// <see cref="ICollection{T}"/>拓展方法
/// </summary>
public static class CollectionsExtensions
{
    #region SyncForEach

    /// <summary>
    /// 遍历数组
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    public static void ForEach(this object[] objs, Action<object> action)
    {
        foreach (var o in objs)
        {
            action(o);
        }
    }

    /// <summary>
    /// 遍历 IEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    public static void ForEach(this IEnumerable<dynamic> objs, Action<object> action)
    {
        foreach (var o in  objs)
        {
            action(o);
        }
    }
    
    /// <summary>
    /// 遍历 IAsyncEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    public static async Task ForEach(this IAsyncEnumerable<dynamic> objs, Action<object> action)
    {
        await foreach (var o in  objs)
        {
            action(o);
        }
    }

    /// <summary>
    /// 遍历集合
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    public static void ForEach(this IList<dynamic> objs, Action<object> action)
    {
        foreach (var o in objs)
        {
            action(o);
        }
    }

    /// <summary>
    /// 遍历数组
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this T[] objs, Action<T> action)
    {
        foreach (var o in objs)
        {
            action(o);
        }
    }

    /// <summary>
    /// 遍历 IEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this IEnumerable<T> objs, Action<T> action)
    {
        foreach (var o in objs)
        {
            action(o);
        }
    }

    /// <summary>
    /// 遍历 IAsyncEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static async Task ForEach<T>(this IAsyncEnumerable<T> objs, Action<T> action)
    {
        await foreach (var o in objs)
        {
            action(o);
        }
    }
    
    /// <summary>
    /// 遍历 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static void ForEach<T>(this IList<T> objs, Action<T> action)
    {
        foreach (var o in objs)
        {
            action(o);
        }
    }

    /// <summary>
    /// 遍历数组并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this object[] objs, Func<object, T> action)
    {
        foreach (var o in objs)
        {
            yield return action(o);
        }
    }

    /// <summary>
    /// 遍历 IEnumerable 并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<dynamic> objs, Func<object, T> action)
    {
        foreach (var o in objs)
        {
            yield return action(o);
        }
    }

    /// <summary>
    /// 遍历 IAsyncEnumerable 并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async IAsyncEnumerable<T> ForEach<T>(this IAsyncEnumerable<dynamic> objs, Func<object, T> action)
    {
        await foreach (var o in objs)
        {
            yield return action(o);
        }
    }
    
    /// <summary>
    /// 遍历List并返回一个新的List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IList<dynamic> objs, Func<object, T> action)
    {
        foreach (var o in objs)
        {
            yield return action(o);
        }
    }


    /// <summary>
    /// 遍历数组并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this T[] objs, Func<T, T> action)
    {
        foreach (var o in objs)
        {
            yield return action(o);
        }
    }

    /// <summary>
    /// 遍历 IEnumerable 并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> objs, Func<T, T> action)
    {
        foreach (var o in objs)
        {
            yield return action(o);
        }
    }

    /// <summary>
    /// 遍历 IAsyncEnumerable 并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async IAsyncEnumerable<T> ForEach<T>(this IAsyncEnumerable<T> objs, Func<T, T> action)
    {
        await foreach (var o in objs)
        {
            yield return action(o);
        }
    }
    
    /// <summary>
    /// 遍历 List 并返回一个新的 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IList<T> objs, Func<T, T> action)
    {
        foreach (var o in objs)
        {
            yield return action(o);
        }
    }
    
    #endregion

    #region AsyncForEach

    /// <summary>
    /// 遍历数组
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    public static async void ForEachAsync(this object[] objs, Action<object> action)
    {
        await Task.Run(() => { Parallel.ForEach(objs, action); });
    }

    /// <summary>
    /// 遍历数组
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static async void ForEachAsync<T>(this T[] objs, Action<T> action)
    {
        await Task.Run(() => { Parallel.ForEach(objs, action); });
    }

    /// <summary>
    /// 遍历 IEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static async void ForEachAsync<T>(this IEnumerable<T> objs, Action<T> action)
    {
        await Task.Run(() => { Parallel.ForEach(objs, action); });
    }

    /// <summary>
    /// 遍历 List
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static async void ForEachAsync<T>(this IList<T> objs, Action<T> action)
    {
        await Task.Run(() => { Parallel.ForEach(objs, action); });
    }

    #endregion

    #region Valid

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> objs)
    {
        return objs == null || objs.Count() == 0;
    }

    #endregion
}