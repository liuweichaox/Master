using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xunit;
using Virgo.Extensions;
using Shouldly;
using System.Linq;

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
        public void CellsToList_Test()
        {
            var dynmaicCell = new string[3, 3]
            {
                {" 姓  名 "," 邮  箱 ","创建时间" },
                {"Jon","123456@gmail.com",DateTime.Now.ToString() },
                {"Allen","98765@gmail.com",DateTime.Now.ToString()}
            };

            var templates = dynmaicCell.CellsToList<ExcelTemplate>();
            templates.Any(x => x.Name == "Jon").ShouldBe(true);
        }

        /// <summary>
        /// <see cref="ExcelExtensions"/>测试带异常
        /// </summary>
        [Fact]
        public void CellsToListWithException_Test()
        {
            var dynmaicCell = new string[3, 3]
            {
                {" 姓  名 "," 邮  箱 ","创建时间" },
                {"Jon","123456@gmail.com","123" },
                {"Allen","98765@gmail.com","abc"}
            };

            try
            {
                var templates = dynmaicCell.CellsToList<ExcelTemplate>();
                templates.Any(x => x.Name == "Jon").ShouldBe(true);
            }
            catch (ExcelException ex)
            {
                ex.ExcelExceptions.Count.ShouldBe(2);
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
