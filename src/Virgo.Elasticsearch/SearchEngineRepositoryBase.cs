using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Elasticsearch
{
    /// <summary>
    /// <see cref="ISearchEngineRepository"/>搜索引擎仓储抽象实现类类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SearchEngineRepositoryBase<T> : ISearchEngineRepository<T> where T : class
    {
        /// <summary>
        /// Elasticsearch客户端
        /// </summary>
        public ElasticClient ElasticClient { get; }
        /// <summary>
        /// 索引名
        /// </summary>
        public IndexName Index { get; }
        /// <summary>
        /// 初始化设置客户端
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="defaultIndex"></param>
        protected SearchEngineRepositoryBase(Uri uri, string defaultIndex = null)
        {
            var connection = new ConnectionSettings(uri).DefaultIndex(defaultIndex);
            Index = defaultIndex;
            ElasticClient = new ElasticClient(connection);
        }
        /// <summary>
        /// 批量插入、更新
        /// </summary>
        /// <param name="document">更新的对象集合</param>
        /// <returns>是否更成功</returns>
        public virtual async Task<bool> BulkAsync(IEnumerable<T> document)
        {
            bool result;
            try
            {
                IBulkRequest bulk = new BulkRequest()
                {
                    Operations = new List<IBulkOperation>(document.Select(doc => new BulkIndexOperation<T>(doc)).Cast<IBulkOperation>().ToList()),
                    Refresh = Refresh.True
                };
                var bulkResponse = await ElasticClient.BulkAsync(bulk);
                result = bulkResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="id">删除的主键</param>
        /// <param name="selector">删除的条件</param>
        /// <returns>是否删除成功</returns>
        public virtual async Task<bool> DeleteAsync(Id id, Func<DeleteDescriptor<T>, IDeleteRequest<T>> selector = null)
        {
            bool result;
            try
            {
                var documentPath = new DocumentPath<T>(id);
                var deleteIndexResponse = await ElasticClient.DeleteAsync(documentPath, selector);
                result = deleteIndexResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据查询删除
        /// </summary>
        /// <param name="selector">删除条件</param>
        /// <returns>是否删除成功</returns>
        public virtual async Task<bool> DeleteByQueryAsync(Func<DeleteByQueryDescriptor<T>, IDeleteByQueryRequest<T>> selector)
        {
            bool result;
            try
            {
                var deleteIndexResponse = await ElasticClient.DeleteByQueryAsync(selector);
                result = deleteIndexResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>是否删除成功</returns>
        public virtual async Task<bool> DeleteIndexAsync(string index)
        {
            bool result;
            try
            {
                var deleteIndexResponse = await ElasticClient.DeleteIndexAsync(index);
                result = deleteIndexResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="objects">删除的对象实体集合</param>
        /// <param name="type">删除的对象类型</param>
        /// <returns>是否删除成功</returns>
        public virtual async Task<bool> DeleteManyAsync(IEnumerable<T> objects, TypeName type = null)
        {
            bool result;
            try
            {
                var deleteManyResponse = await ElasticClient.DeleteManyAsync<T>(objects, Index, type);
                result = deleteManyResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据ID判断对象是否存在
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="selector">其他条件</param>
        /// <returns>对象是否存在</returns>
        public virtual async Task<bool> DocumentExistsAsync(Id id, Func<DocumentExistsDescriptor<T>, DocumentExistsRequest<T>> selector)
        {
            bool result;
            try
            {
                var document = new DocumentPath<T>(id);
                var existsResponse = await ElasticClient.DocumentExistsAsync<T>(document, selector);
                result = existsResponse.Exists;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 根据ID获一个对象信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="selector">其他条件</param>
        /// <returns>对象信息</returns>
        public virtual async Task<T> GetAsync(Id id, Func<GetDescriptor<T>, GetRequest<T>> selector = null)
        {
            T result;
            try
            {
                var document = new DocumentPath<T>(id);
                var getResponse = await ElasticClient.GetAsync<T>(document, selector);
                var json = JsonConvert.SerializeObject(getResponse.Source);
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 根据多个ID获取对象信息
        /// </summary>
        /// <param name="ids">主键集合</param>
        /// <param name="type">类型名</param>
        /// <returns>对象集合</returns>
        public virtual async Task<List<T>> GetManyAsync(IEnumerable<string> ids, TypeName type = null)
        {
            List<T> result;
            try
            {
                var getHits = await ElasticClient.GetManyAsync<T>(ids, Index, type);
                var json = JsonConvert.SerializeObject(getHits.Select(x => x.Source));
                result = JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 插入单挑数据
        /// </summary>
        /// <param name="document">插入的对象</param>
        /// <param name="selector"></param>
        /// <returns>是否插入成功</returns>
        public virtual async Task<bool> IndexAsync(T document, Func<IndexDescriptor<T>, IIndexRequest<T>> selector = null)
        {
            bool result;
            try
            {
                var indexResponse = await ElasticClient.IndexAsync(document, selector);
                result = indexResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 批量插入多条数据
        /// </summary>
        /// <param name="document">插入的对象集合</param>
        /// <param name="type">类型</param>
        /// <returns>是否插入成功</returns>
        public virtual async Task<bool> IndexManyAsync(IEnumerable<T> document, TypeName type = null)
        {
            bool result;
            try
            {
                var indexResponse = await ElasticClient.IndexManyAsync(document, Index, type);
                result = indexResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 刷新索引
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>返回是否刷新成功</returns>
        public virtual async Task<bool> RefreshAsync(string index)
        {
            bool result;
            try
            {
                var refreshResponse = await ElasticClient.RefreshAsync(index);
                result = refreshResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="selector">搜索条件</param>
        /// <returns>对象型集合</returns>
        public virtual async Task<List<T>> SearchAsync(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
        {
            List<T> result = null;
            try
            {
                var searchResponse = await ElasticClient.SearchAsync<T>(selector);
                if (searchResponse.ApiCall.Success)
                {
                    var json = JsonConvert.SerializeObject(searchResponse.Documents);
                    result = JsonConvert.DeserializeObject<List<T>>(json);
                }
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 更新执行字段
        /// </summary>
        /// <param name="document">需要被更新的对象</param>
        /// <param name="selector">变更条件</param>
        /// <returns>变更是否成功</returns>
        public virtual async Task<bool> UpdateAsync(T document, Func<UpdateDescriptor<T, T>, IUpdateRequest<T, T>> selector)
        {
            bool result;
            try
            {
                var documentPath = new DocumentPath<T>(document);
                var updateResponse = await ElasticClient.UpdateAsync<T>(documentPath, selector);
                result = updateResponse.ApiCall.Success;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}
