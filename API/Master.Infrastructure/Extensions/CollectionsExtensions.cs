namespace Master.Infrastructure.Extensions;

/// <summary>
/// <see cref="ICollection{T}"/>拓展方法
/// </summary>
public static class CollectionsExtensions
{
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
    public static async IAsyncEnumerable<T> ForEachAsync<T>(this IAsyncEnumerable<dynamic> objs, Func<object, T> action)
    {
        await foreach (var o in objs)
        {
            yield return action(o);
        }
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
    /// 遍历 IAsyncEnumerable
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="action">回调方法</param>
    /// <typeparam name="T"></typeparam>
    public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> objs, Action<T> action)
    {
        await foreach (var o in objs)
        {
            action(o);
        }
    }
    
    /// <summary>
    /// 是否为空
    /// </summary>
    /// <param name="objs"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? objs)
    {
        return objs == null || !objs.Any();
    }
}