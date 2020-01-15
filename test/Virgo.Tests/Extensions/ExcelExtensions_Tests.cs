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
            var temps = new List<ExcelTemplate>()
            {
                new ExcelTemplate()
                {
                    CreateTime=DateTime.Now,
                    Email="12345@gamil.com",
                    Name="Jon"
                },
                new ExcelTemplate()
                {
                    CreateTime=DateTime.Now,
                    Email="54321@gamil.com",
                    Name="Allen"
                }
            };
            var path = "D:\\ExcelTest.xlxs";
            temps.ExportToFile(path);
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
            var temps = new List<ExcelTemplate>()
            {
                new ExcelTemplate()
                {
                    CreateTime=DateTime.Now,
                    Email="12345@gamil.com",
                    Name="Jon"
                },
                new ExcelTemplate()
                {
                    CreateTime=DateTime.Now,
                    Email="54321@gamil.com",
                    Name="Allen"
                }
            };
            using (var stream = temps.ExportToStream())
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
            var path = "D:\\ExcelTest.xlxs";
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var templates = stream.ReadExcel<ExcelTemplate>();
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
        /// 创建时间 
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
