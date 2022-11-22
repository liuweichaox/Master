using System;
using System.Collections.Generic;
using System.Text;
using Virgo.DependencyInjection;

namespace Virgo.Elasticsearch.Tests
{
    /// <summary>
    /// <see cref="BeautySpot"/>仓储实现
    /// </summary>
    public class BeautySpotRepository : ElasticsearchRepositoryBase<BeautySpot>, IBeautySpotRepository, ITransientDependency
    {
        public BeautySpotRepository(IElasticClientFactory factory) : base(factory)
        {

        }
    }
}
