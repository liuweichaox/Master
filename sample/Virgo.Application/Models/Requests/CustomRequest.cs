using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Application.Models.Requests
{
    /// <summary>
    /// 自定义请求实体示例
    /// </summary>
    public class CustomRequest
    {
        /// <summary>
        /// Guid Xml GO GO GO
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 变量
        /// </summary>
        public string Name { get; set; }
    }
}
