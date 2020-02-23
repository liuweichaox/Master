using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Domain.Models
{
    public class CustomEntity
    {
        /// <summary>
        /// Guid Xml GO GO GO
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 变量
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }
    }
}
