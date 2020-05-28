using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Virgo.IO;

namespace Virgo.Extensions
{
    /// <summary>
    /// Excel拓展
    /// </summary>
    public static class ExcelExtensions
    {
        #region 导入
        /// <summary>
        /// 将Excel转换为<see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        /// <exception cref="ExcelException">Excel错误消息类型</exception>
        public static List<T> ExcelToList<T>(this Stream stream, int worksheet = 0) where T : class, new()
        {
            if (stream == null)
                throw new ArgumentException(nameof(stream));
            using var pck = new ExcelPackage(stream);
            var ws = pck.Workbook.Worksheets[worksheet];
            var minColumnNum = ws.Dimension.Start.Column; //工作区开始列
            var maxColumnNum = ws.Dimension.End.Column; //工作区结束列
            var minRowNum = ws.Dimension.Start.Row; //工作区开始行号
            var maxRowNum = ws.Dimension.End.Row; //工作区结束行号
            var errors = new List<ExcelExceptioInfo>();
            var list = new List<T>();
            var propertyPosition = new Dictionary<int, string>();
            var propertyInfos = typeof(T).GetProperties();
            if (propertyInfos.Length != maxColumnNum)
                throw new Exception($"Excel格式错误，无法将该文件转换为List<{typeof(T).Name}>对象！");
            for (int row = minRowNum; row <= maxRowNum; row++)
            {
                var rowInstance = Activator.CreateInstance<T>();
                for (int col = minColumnNum; col <= maxColumnNum; col++)
                {
                    var cell = ws.Cells[row, col].Value.ToString();
                    if (row == minRowNum)
                    {
                        var propertyName = propertyInfos.SingleOrDefault(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description == cell)?.Name;
                        if (propertyName != null)
                        {
                            propertyPosition.Add(col, propertyName);
                        }
                    }
                    else
                    {
                        var propertyName = propertyPosition.GetValueOrDefault(col);
                        if (propertyName != null)
                        {
                            var property = propertyInfos.SingleOrDefault(x => x.Name == propertyName);
                            try
                            {
                                var value = Convert.ChangeType(cell, property.PropertyType);
                                property.SetValue(rowInstance, value);
                            }
                            catch (Exception ex)
                            {
                                var msg = $"{row + 1}行{col + 1}列单元格：【{cell ?? ""}】转换为【{property.PropertyType.Name}】类型异常";
                                var error = new ExcelExceptioInfo(row, col, msg, ex.Message, ex.StackTrace);
                                errors.Add(error);
                            }
                        }
                    }
                }
                if (row != minRowNum)
                {
                    list.Add(rowInstance);
                }
            }
            if (errors.Any())
            {
                throw new ExcelException(errors);
            }
            return list;
        }
        #endregion

        #region 导出
        /// <summary>
        /// 导出Excel.将<see cref="List{T}"/>转为为<see cref="ExcelPackage"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ExcelPackage ExportToExcel<T>(this List<T> list)
        {
            if (list == null || list.Any() == false)
                throw new ArgumentException(nameof(list));
            var type = typeof(T);
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(type.Name);
            var propertyInfos = type.GetProperties();
            var rows = list.Count;
            var cols = propertyInfos.Length;
            for (int row = 0; row < rows; row++)
            {
                var item = list[row];
                for (int col = 0; col < cols; col++)
                {
                    var property = propertyInfos[col];
                    if (row == 0)
                    {
                        var title = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                        sheet.SetValue(row + 1, col + 1, title);//第一行设置标题
                    }
                    var val = Convert.ChangeType(property.GetValue(item), property.PropertyType).ToString();
                    var cell = sheet.Cells[row + 2, col + 1];//第一条数据从第二行开始追加
                    cell.Value = val;
                    if (val.IsNumeric() && val.Length > 11)
                    {
                        cell.Style.Numberformat.Format = "@";
                    }
                }
            }
            return package;
        }

        /// <summary>
        /// 导出Excel.将<see cref="List{T}"/>转为为<see cref="MemoryStream"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static MemoryStream ExportToStream<T>(this List<T> list)
        {
            if (list == null || list.Any() == false)
                throw new ArgumentException(nameof(list));
            using var package = ExportToExcel(list);
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
        /// <summary>
        /// 导出Excel.将<see cref="List{T}"/>转为为<see cref="File"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="path"></param>
        public static void ExportToFile<T>(this List<T> list, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));
            FileHelper.DeleteIfExists(path);
            using var package = ExportToExcel(list);
            using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            package.SaveAs(stream);
        }
        #endregion        
    }

    /// <summary>
    /// Excel异常模板
    /// </summary>
    public class ExcelExceptioInfo
    {
        public ExcelExceptioInfo()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="message"></param>
        /// <param name="originalMessage"></param>
        /// <param name="stackTrace"></param>
        public ExcelExceptioInfo(int row, int column, string message, string originalMessage, string stackTrace)
        {
            Row = row;
            Column = column;
            Message = message;
            OriginalMessage = originalMessage;
            StackTrace = stackTrace;
        }
        /// <summary>
        /// 行坐标
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// 列坐标
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 原始异常
        /// </summary>
        public string OriginalMessage { get; set; }
        /// <summary>
        /// 堆栈跟踪
        /// </summary>
        public string StackTrace { get; set; }
    }
    /// <summary>
    /// Excel异常
    /// </summary>
    public class ExcelException : Exception
    {
        public List<ExcelExceptioInfo> ExcelExceptions;
        public ExcelException(List<ExcelExceptioInfo> excelExceptions)
        {
            ExcelExceptions = excelExceptions;
        }
    }
}
