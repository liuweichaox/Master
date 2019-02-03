using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Virgo.Dapper
{
    public interface IDbConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// 命令超时时间,单位:秒
        /// </summary>
        int? CommandTimeout { get; set; }
    }
}
