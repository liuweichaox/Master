using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Virgo.Data
{
    /// <summary>
    /// MsSql工具类
    /// </summary>
    public static class MsSqlHelper
    {
        /// <summary>
        /// 批量插入(同步方法)
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="dataTable"></param>
        public static void BulkCopy(SqlConnection sqlConnection, DataTable dataTable)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection))
            {
                sqlBulkCopy.DestinationTableName = dataTable.TableName;
                sqlBulkCopy.WriteToServer(dataTable);
            }
        }
        /// <summary>
        /// 批量插入(异步方法)
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="dataTable"></param>
        public static async Task BulkCopyAsync(SqlConnection sqlConnection, DataTable dataTable)
        {
            using (var sqlBulkCopy = new SqlBulkCopy(sqlConnection))
            {
                sqlBulkCopy.DestinationTableName = dataTable.TableName;
                await sqlBulkCopy.WriteToServerAsync(dataTable);
            }
        }
    }
}
