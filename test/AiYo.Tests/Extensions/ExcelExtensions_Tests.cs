using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xunit;
using Virgo.Extensions;
using Shouldly;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Virgo.Tests.Extensions
{
    /// <summary>
    /// <see cref="ExcelExtensions"/>测试
    /// </summary>
    public class ExcelExtensions_Tests
    {
        /// <summary>
        /// 导出Excel.将<see cref="List{T}"/>转为为<see cref="File"/>
        /// </summary>
        [Fact]
        public void ExportExcelToFile_Test()
        {
            var temps = new ConcurrentBag<ExcelTemplate>();
            Parallel.For(0, 33333, i =>
            {
                var item1 = new ExcelTemplate()
                {
                    CreateTime = DateTime.Now,
                    Email = "12345@gamil.com",
                    Name = "Jon",
                    Card = "1234567890123456"
                };

                var item2 = new ExcelTemplate()
                {
                    CreateTime = DateTime.Now,
                    Email = "54321@gamil.com",
                    Name = "Allen",
                    Card = "6543210987654321"
                };

                var item3 = new ExcelTemplate()
                {
                    CreateTime = DateTime.Now,
                    Email = "666666@gamil.com",
                    Name = "Lucy",
                    Card = "6543210987654321"
                };
                temps.Add(item1);
                temps.Add(item2);
                temps.Add(item3);
            });
            var path = "D:\\ExcelTest.xlsx";
            temps.ToList().ToExcelFileFromObjects(path);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// 导出Excel，将<see cref="List{T}"/>转为为<see cref="MemoryStream"/>
        /// </summary>
        [Fact]
        public void ExportExcelToStream_Test()
        {
            var temps = new ConcurrentBag<ExcelTemplate>();
            Parallel.For(0, 33333, i =>
              {
                  var item1 = new ExcelTemplate()
                  {
                      CreateTime = DateTime.Now,
                      Email = "12345@gamil.com",
                      Name = "Jon",
                      Card = "1234567890123456"
                  };

                  var item2 = new ExcelTemplate()
                  {
                      CreateTime = DateTime.Now,
                      Email = "54321@gamil.com",
                      Name = "Allen",
                      Card = "6543210987654321"
                  };

                  var item3 = new ExcelTemplate()
                  {
                      CreateTime = DateTime.Now,
                      Email = "666666@gamil.com",
                      Name = "Lucy",
                      Card = "6543210987654321"
                  };
                  temps.Add(item1);
                  temps.Add(item2);
                  temps.Add(item3);
              });
            using (var stream = temps.ToList().ToStreamFromObjects())
            {
                stream.ShouldNotBeNull();
            }
        }

        /// <summary>
        /// 读取Excel.将Excel转换为<see cref="List{T}"/>
        /// </summary>
        [Fact]
        public void ReadExcel_Test()
        {
            var path = "D:\\ExcelTest.xlsx";
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var templates = stream.ToObjectsFromExcel<ExcelTemplate>();
                templates.ShouldNotBeNull();
            }
        }
    }

    /// <summary>
    /// Excel导入模板
    /// </summary>
    public class ExcelTemplate
    {
        /// <summary>
        /// 姓名 
        /// </summary>
        [Description(" 姓  名 ")]
        public string Name { get; set; }
        /// <summary>
        /// 邮箱 
        /// </summary>
        [Description(" 邮  箱 ")]
        public string Email { get; set; }
        /// <summary>
        /// 身份证 
        /// </summary>
        [Description(" 身份证 ")]
        public string Card { get; set; }
        /// <summary>
        /// 创建时间 
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
