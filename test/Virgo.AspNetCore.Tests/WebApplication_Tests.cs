using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Virgo.UserInterface;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Virgo.AspNetCore.Tests
{
    /// <summary>
    /// Web集成测试
    /// </summary>
    public class WebApplication_Tests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        public WebApplication_Tests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(build =>
            {
                build.ConfigureServices(services =>
                {
                    services.AddSingleton<IService, Service>();
                });
            });
        }

        /// <summary>
        /// 调用接口
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAsync()
        {
            //单元测试的3A原则：

            // Arrange 设置测试数据、变量、环境等
            var client = _factory.CreateClient();

            // Act 调用要测试的函数、代码
            var response = await client.GetAsync("/api/v1.0/Value/Add?a=1&b=2");

            // Assert 验证输出是否是预期结果
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("3", await response.Content.ReadAsStringAsync());


            var service = _factory.Services.GetService<IService>();
        }
    }
    public interface IService
    {
        int Age { get; set; }
        string Call();
    }
    public class Service : IService
    {
        public int Age { get; set; } = 100;

        public string Call()
        {
            return "Test Return";
        }
    }
}
