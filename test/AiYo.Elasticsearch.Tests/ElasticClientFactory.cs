using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using Virgo.DependencyInjection;

namespace Virgo.Elasticsearch.Tests
{
    /// <summary>
    /// <see cref="ElasticClient"/>工厂
    /// </summary>
    public class ElasticClientFactory : IElasticClientFactory, ITransientDependency
    {
        public ElasticClient Create()
        {
            //var uri = new Uri("http://elastic:123456@localhost:9200");url内的身份验证
            var uri = new Uri("http://localhost:9200");
            var nodes = new Node[]
            {
                new Node(uri)
            };
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool).BasicAuthentication("elastic", "123456").DefaultIndex("virgo");
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
