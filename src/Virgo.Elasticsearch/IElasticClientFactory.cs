using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Elasticsearch
{
    /// <summary>
    /// <see cref="ElasticClient"/>创建工厂
    /// </summary>
    public interface IElasticClientFactory
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        ElasticClient Create();
    }
}
