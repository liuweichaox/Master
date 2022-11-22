using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Application.Models.Responses
{
    /// <summary>
    /// 自定义响应实体示例
    /// </summary>
    public class CostomResponse
    {
        /// <summary>
        /// XML ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// XML DATE
        /// </summary>
        public DateTime Date { get; set; }
    }
}
