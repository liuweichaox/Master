using Microsoft.EntityFrameworkCore;

namespace Master.Infrastructure.Api;

/// <summary>
/// 分页结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class PagedResult<TResult>
{
    public PagedResult() { }

    public PagedResult(IEnumerable<TResult> items, int totalCount, int pageIndex, int pageSize)
    {
        Results = items;
        TotalCount = totalCount;
        PageCount = (totalCount + pageSize - 1) / pageSize;
        PageIndex = pageIndex;
        PageSize = pageSize;
        HasPrev = pageIndex > 1;
        HasNext = pageIndex < PageCount;
    }
    /// <summary>
    /// 分页集合
    /// </summary>
    public virtual IEnumerable<TResult> Results { get; private set; }
    /// <summary>
    /// 总数据量
    /// </summary>
    public virtual int TotalCount { get; private set; }
    /// <summary>
    /// 总页码
    /// </summary>
    public virtual int PageCount { get; private set; }
    /// <summary>
    /// 当前页码
    /// </summary>
    public virtual int PageIndex { get; private set; }
    /// <summary>
    /// 页码大小
    /// </summary>
    public virtual int PageSize { get; private set; }
    /// <summary>
    /// 是否有上一页
    /// </summary>
    public virtual bool HasPrev { get; private set; }
    /// <summary>
    /// 是否有下一页
    /// </summary>
    public virtual bool HasNext { get; private set; }
}