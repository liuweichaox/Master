using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Virgo.Extensions;
namespace Virgo.Data
{
    /// <summary>
    /// MySql工具类
    /// </summary>
    public static class MySqlHelper
    {
        /// <summary>
        /// 批量导入(同步方法)
        /// </summary> 
        /// <param name="_mySqlConnection"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int BulkLoad(MySqlConnection _mySqlConnection, DataTable table)
        {
            var tempPath = Path.GetTempFileName();
            var contents = table.ToCsv();
            File.WriteAllText(tempPath, contents);
            var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
            MySqlBulkLoader bulk = new MySqlBulkLoader(_mySqlConnection)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = tempPath,
                NumberOfLinesToSkip = 0,
                TableName = table.TableName,

            };
            bulk.Columns.AddRange(columns);
            var result = bulk.Load();
            File.Delete(tempPath);
            return result;
        }
        /// <summary>
        /// 批量导入(异步方法)
        /// </summary>
        /// <param name="_mySqlConnection"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static async Task<int> BulkLoadAsync(MySqlConnection _mySqlConnection, DataTable table)
        {
            var tempPath = Path.GetTempFileName();
            var contents = table.ToCsv();
            File.WriteAllText(tempPath, contents);
            var columns = table.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
            MySqlBulkLoader bulk = new MySqlBulkLoader(_mySqlConnection)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = tempPath,
                NumberOfLinesToSkip = 0,
                TableName = table.TableName,

            };
            bulk.Columns.AddRange(columns);
            var result = await bulk.LoadAsync();
            File.Delete(tempPath);
            return result;
        }
    }
}
