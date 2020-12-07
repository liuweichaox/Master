using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Virgo.UserInterface;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Virgo.AspNetCore.Tests
{
    /// <summary>
    /// Web���ɲ���
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
        /// ���ýӿ�
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAsync()
        {
            //��Ԫ���Ե�3Aԭ��

            // Arrange ���ò������ݡ�������������
            var client = _factory.CreateClient();

            // Act ����Ҫ���Եĺ���������
            var response = await client.GetAsync("/api/v1.0/Value/Add?a=1&b=2");

            // Assert ��֤����Ƿ���Ԥ�ڽ��
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
