using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xunit;
using Virgo.Extensions;
using Shouldly;
using System.Linq;
using System.IO;

namespace Virgo.Tests.Extensions
{
    /// <summary>
    /// <see cref="ExcelExtensions"/>测试
    /// </summary>
    public class ExcelExtensions_Tests
    {
        /// <summary>
        /// 动态单元格转换
        /// </summary>
        [Fact]
        public void ExcelToList_Test()
        {
            var dynmaicCell = new string[3, 3]
            {
                {" 姓  名 "," 邮  箱 ","创建时间" },
                {"Jon","123456@gmail.com",DateTime.Now.ToString() },
                {"Allen","98765@gmail.com",DateTime.Now.ToString()}
            };

            var templates = dynmaicCell.ExcelToList<ExcelTemplate>();
            templates.Any(x => x.Name == "Jon").ShouldBe(true);
        }

        /// <summary>
        /// <see cref="ExcelExtensions"/>测试带异常
        /// </summary>
        [Fact]
        public void ExcelToListWithException_Test()
        {
            var dynmaicCell = new string[3, 3]
            {
                {" 姓  名 "," 邮  箱 ","创建时间" },
                {"Jon","123456@gmail.com","123" },
                {"Allen","98765@gmail.com","abc"}
            };

            try
            {
                var templates = dynmaicCell.ExcelToList<ExcelTemplate>();
                templates.Any(x => x.Name == "Jon").ShouldBe(true);
            }
            catch (ExcelException ex)
            {
                ex.ExcelExceptions.Count.ShouldBe(2);
            }


        }

        /// <summary>
        /// 导出Excel测试，创建Excel流
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
            using var stream = temps.ExportToStream();
            stream.Length.ShouldBeGreaterThan(0);
            var list = stream.ReadExcel<ExcelTemplate>();
            list.ShouldNotBeNull();
        }

        /// <summary>
        /// 导出Excel测试，导出为文件
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
            using StreamReader stream = new StreamReader(path);
            stream.ReadToEnd().ShouldNotBeNull();
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
