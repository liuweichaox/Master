using OfficeOpenXml;
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
        /// 读取Excel，将<see cref="Stream"/>转换为<see cref="string[][]"/>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static string[][] ReadExcel(this Stream stream, int worksheet = 0)
        {
            if (stream == null)
                throw new ArgumentException(nameof(stream));
            using var pck = new ExcelPackage(stream);
            var ws = pck.Workbook.Worksheets[worksheet];
            var minColumnNum = ws.Dimension.Start.Column; //工作区开始列
            var maxColumnNum = ws.Dimension.End.Column; //工作区结束列
            var minRowNum = ws.Dimension.Start.Row; //工作区开始行号
            var maxRowNum = ws.Dimension.End.Row; //工作区结束行号
            string[][] cells = new string[maxRowNum][];
            for (var row = minRowNum; row <= maxRowNum; row++)
            {
                string[] rowItem = new string[maxColumnNum];
                for (int col = minColumnNum; col <= maxColumnNum; col++)
                {
                    rowItem[col - 1] = ws.GetValue(row, col).ToString();
                }
                cells[row - 1] = rowItem;
            }
            return cells;
        }

        /// <summary>
        /// 读取Excel，将<see cref="string[][]"/>转换为<see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cells"></param>
        /// <returns></returns>
        /// <exception cref="ExcelException"></exception>
        public static List<T> ExcelToList<T>(this string[][] cells) where T : class, new()
        {
            if (cells == null)
                throw new ArgumentException(nameof(cells));
            var errors = new List<ExcelExceptionTemplate>();
            var list = new List<T>();
            var propertyPosition = new Dictionary<int, string>();
            var propertyInfos = typeof(T).GetProperties();
            for (int row = 0; row < cells.Length; row++)
            {
                var rowInstance = Activator.CreateInstance<T>();
                for (int col = 0; col < cells.Length; col++)
                {
                    var cell = cells[row][col];
                    if (row == 0)
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
                                var error = new ExcelExceptionTemplate()
                                {
                                    Column = col,
                                    Row = row,
                                    OriginalMessage = ex.Message,
                                    StackTrace = ex.StackTrace,
                                    Message = $"{row + 1}行{col + 1}列单元格：【{cell ?? ""}】转换为【{property.PropertyType.Name}】类型异常"
                                };
                                errors.Add(error);
                            }
                        }
                    }
                }
                if (row != 0)
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
        /// <summary>
        /// 读取Excel.将Excel转换为<see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static List<T> ReadExcel<T>(this Stream stream, int worksheet = 0) where T : class, new()
        {
            if (stream == null)
                throw new ArgumentException(nameof(stream));
            var cells = ReadExcel(stream, worksheet);
            return ExcelToList<T>(cells);
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
                        var value = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                        sheet.SetValue(row + 1, col + 1, value);//第一行设置标题
                    }
                    var objVal = Convert.ChangeType(property.GetValue(item), property.PropertyType).ToString();
                    sheet.SetValue(row + 2, col + 1, objVal);//第一条数据从第二行开始追加
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
    public class ExcelExceptionTemplate
    {
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
        public List<ExcelExceptionTemplate> ExcelExceptions;
        public ExcelException(List<ExcelExceptionTemplate> excelExceptions)
        {
            ExcelExceptions = excelExceptions;
        }
    }
}
