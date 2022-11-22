using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Elasticsearch.Tests
{
    /// <summary>
    /// <see cref="BeautySpot"/>仓储接口
    /// </summary>
    public interface IBeautySpotRepository : IElasticsearchRepository<BeautySpot>
    {
    }
}
