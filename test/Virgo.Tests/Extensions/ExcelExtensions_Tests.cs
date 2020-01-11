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
        public void DynmaicCellToList_Test()
        {
            var dynmaicCell = new string[3, 3]
            {
                {" 姓  名 "," 邮  箱 ","创建时间" },
                {"Jon","123456@gmail.com",DateTime.Now.ToString() },
                {"Allen","98765@gmail.com",DateTime.Now.ToString()}
            };
            var templates = dynmaicCell.DynmaicCellToList<ExcelTemplate>();
            templates.Any(x => x.Name == "Jon").ShouldBe(true);
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
