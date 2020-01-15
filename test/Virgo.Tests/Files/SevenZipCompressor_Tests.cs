using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Virgo.Files;
using Virgo.TestBase;
using Xunit;

namespace Virgo.Tests.Files
{
    /// <summary>
    /// <see cref="ISevenZipCompressor"/>测试类
    /// </summary>
    public class SevenZipCompressor_Tests : TestBaseWithServiceCollection
    {
        /// <summary>
        /// <see cref="IServiceCollection"/>服务容器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// <see cref="ISevenZipCompressor"/>实例
        /// </summary>
        private readonly ISevenZipCompressor _sevenZipCompressor;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SevenZipCompressor_Tests()
        {
            _serviceProvider = Building(build =>
              {
                  build.AddSevenZipCompressor();
              });
            _sevenZipCompressor = The<ISevenZipCompressor>();
        }

        /// <summary>
        /// 解压文件，自动检测压缩包类型
        /// </summary>
        [Fact]
        public void Decompress_Test()
        {
            _sevenZipCompressor.Decompress("D:\\test.zip", "D:\\Zip");
        }
        /// <summary>
        /// 解压文件，自动检测压缩包类型
        /// </summary>
        [Fact]
        public void Extract_Test()
        {
            _sevenZipCompressor.Extract("D:\\test.zip", "D:\\Zip");
        }
        /// <summary>
        /// 解压rar文件
        /// </summary>
        [Fact]
        public void UnRar_Test()
        {
            _sevenZipCompressor.UnRar("D:\\test.Rar", "D:\\Rar");
        }
        /// <summary>
        /// 压缩多个文件
        /// </summary>
        [Fact]
        public void Zip_Test()
        {
            var files = new List<string>()
            {
                @"D:\A.TXT",
                @"D:\B.TXT"
            };
            var zipName = "D:\\test.zip";
            _sevenZipCompressor.Zip(files, zipName);
            using (FileStream stream = File.OpenRead(zipName))
            {
                Assert.True(stream.Length > 0);
            }
        }
        /// <summary>
        /// 将多个文件压缩到一个文件流中，可保存为zip文件，方便于web方式下载
        /// </summary>
        [Fact]
        public void ZipStream_Test()
        {
            var files = new List<string>()
            {
                @"D:\A.TXT",
                @"D:\B.TXT"
            };
            var zipName = "D:\\test.zip";
            var stream = _sevenZipCompressor.ZipStream(files, zipName);
            Assert.True(stream.Length > 0);
        }
    }
}
