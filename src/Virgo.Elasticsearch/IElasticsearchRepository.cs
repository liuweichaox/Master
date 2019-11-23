using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Virgo.Elasticsearch
{
    /// <summary>
    /// 搜索引擎仓储基类接口
    /// </summary>
    public interface IElasticsearchRepository<T> where T : class
    {
        /// <summary>
        /// Elasticsearch客户端
        /// </summary>
        ElasticClient ElasticClient { get; }

        /// <summary>
        /// 索引名
        /// </summary>
        IndexName Index { get; }
        /// <summary>
        /// 批量插入、更新
        /// </summary>
        /// <param name="document">更新的对象集合</param>
        /// <returns>是否更成功</returns>
        Task<bool> BulkAsync(IEnumerable<T> document);
        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="id">删除的主键</param>
        /// <param name="selector">删除的条件</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteAsync(Id id, Func<DeleteDescriptor<T>, IDeleteRequest<T>> selector = null);
        /// <summary>
        /// 根据查询删除
        /// </summary>
        /// <param name="selector">删除条件</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteByQueryAsync(Func<DeleteByQueryDescriptor<T>, IDeleteByQueryRequest<T>> selector);
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteIndexAsync(string index);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="objects">删除的对象实体集合</param>
        /// <param name="type">删除的对象类型</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteManyAsync(IEnumerable<T> objects, TypeName type = null);
        /// <summary>
        /// 根据ID判断对象是否存在
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="selector">其他条件</param>
        /// <returns>对象是否存在</returns>
        Task<bool> DocumentExistsAsync(Id id, Func<DocumentExistsDescriptor<T>, DocumentExistsRequest<T>> selector);
        /// <summary>
        /// 根据ID获一个对象信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="selector">其他条件</param>
        /// <returns>对象信息</returns>
        Task<T> GetAsync(Id id, Func<GetDescriptor<T>, GetRequest<T>> selector = null);
        /// <summary>
        /// 根据多个ID获取对象信息
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <param name="type">类型名</param>
        /// <returns>对象集合</returns>
        Task<List<T>> GetManyAsync(IEnumerable<string> ids, TypeName type = null);
        /// <summary>
        /// 插入单挑数据
        /// </summary>
        /// <param name="document">插入的对象</param>
        /// <param name="selector"></param>
        /// <returns>是否插入成功</returns>
        Task<bool> IndexAsync(T document, Func<IndexDescriptor<T>, IIndexRequest<T>> selector = null);
        /// <summary>
        /// 批量插入多条数据
        /// </summary>
        /// <param name="document">插入的对象集合</param>
        /// <param name="type">类型</param>
        /// <returns>是否插入成功</returns>
        Task<bool> IndexManyAsync(IEnumerable<T> document, TypeName type = null);
        /// <summary>
        /// 刷新索引
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>返回是否刷新成功</returns>
        Task<bool> RefreshAsync(string index);
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="selector">搜索条件</param>
        /// <returns>对象型集合</returns>
        Task<List<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector = null);
        /// <summary>
        /// 更新执行字段
        /// </summary>
        /// <param name="document">需要被更新的对象</param>
        /// <param name="selector">变更条件</param>
        /// <returns>变更是否成功</returns>
        Task<bool> UpdateAsync(T document, Func<UpdateDescriptor<T, T>, IUpdateRequest<T, T>> selector);
    }
}
