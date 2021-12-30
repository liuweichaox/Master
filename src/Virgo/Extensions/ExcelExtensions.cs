using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Virgo.IO;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Virgo.Extensions
{
    /// <summary>
    /// Excel拓展
    /// </summary>
    public static class ExcelExtensions
    {
        static ExcelExtensions()
        {  
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        #region 导入
        /// <summary>
        /// 将Excel转换为<see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        /// <exception cref="ExcelException">Excel错误消息类型</exception>
        public static List<T> ToObjectsFromExcel<T>(this Stream stream, int worksheet = 0) where T : class, new()
        {
            if (stream == null)
                throw new ArgumentException(nameof(stream));
            using var pck = new ExcelPackage(stream);
            var ws = pck.Workbook.Worksheets[worksheet];
            var minColumnNum = ws.Dimension.Start.Column; //工作区开始列
            var maxColumnNum = ws.Dimension.End.Column; //工作区结束列
            var minRowNum = ws.Dimension.Start.Row; //工作区开始行号
            var maxRowNum = ws.Dimension.End.Row; //工作区结束行号
            var errors = new List<ExcelAbnormalInfo>();
            var list = new List<T>();
            var propertyPosition = new Dictionary<int, string>();
            var propertyInfos = typeof(T).GetProperties();
            if (propertyInfos.Length != maxColumnNum)
                throw new Exception($"Excel格式错误，无法将该文件转换为List<{typeof(T).Name}>对象！");
            for (var row = minRowNum; row <= maxRowNum; row++)
            {
                var rowInstance = Activator.CreateInstance<T>();
                for (var col = minColumnNum; col <= maxColumnNum; col++)
                {
                    var cell = ws.Cells[row, col].Value.ToString();
                    if (row == minRowNum)
                    {
                        var propertyName = propertyInfos.FirstOrDefault(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description == cell)?.Name;
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
                            var property = propertyInfos.FirstOrDefault(x => x.Name == propertyName);
                            try
                            {
                                var type = property?.PropertyType;
                                var nullableType = Nullable.GetUnderlyingType(type) ?? type;
                                var value = cell == null ? null : Convert.ChangeType(cell, nullableType);
                                property?.SetValue(rowInstance, value);
                            }
                            catch (Exception ex)
                            {
                                var msg = $"{row + 1}行{col + 1}列单元格：【{cell ?? ""}】转换为【{property?.PropertyType.Name}】类型异常";
                                var error = new ExcelAbnormalInfo(row, col, msg, ex.Message, ex.StackTrace);
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
        public static ExcelPackage ToExcelFromObjects<T>(this List<T> list)
        {
            if (list == null || list.Any() == false)
                throw new ArgumentException(nameof(list));
            var type = typeof(T);
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Sheet1");
            var propertyInfos = type.GetProperties();
            var rows = list.Count;
            var cols = propertyInfos.Length;
            for (int row = 0; row < rows; row++)
            {
                var item = list[row];
                for (var col = 0; col < cols; col++)
                {
                    var property = propertyInfos[col];
                    if (row == 0)
                    {
                        var title = property.GetCustomAttribute<DescriptionAttribute>()?.Description ?? property.Name;
                        var firstCell = sheet.Cells[row + 1, col + 1];
                        firstCell.Value = title; //第一行设置标题
                        firstCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    var val = Convert.ChangeType(property.GetValue(item), property.PropertyType).ToString();
                    var cell = sheet.Cells[row + 2, col + 1];//第一条数据从第二行开始追加
                    cell.Value = val;
                    cell.Style.Numberformat.Format = "@";
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
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
        public static MemoryStream ToStreamFromObjects<T>(this List<T> list)
        {
            if (list == null || list.Any() == false)
                throw new ArgumentException(nameof(list));
            using var package = ToExcelFromObjects(list);
            var stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
        /// <summary>
        /// 导出Excel.将<see cref="List{T}"/>转为为<see cref="File"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="path"></param>
        public static void ToExcelFileFromObjects<T>(this List<T> list, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));
            FileHelper.DeleteIfExists(path);
            using var package = ToExcelFromObjects(list);
            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            package.SaveAs(stream);
            stream.Dispose();
        }
        #endregion        
    }

    /// <summary>
    /// Excel异常模板
    /// </summary>
    public class ExcelAbnormalInfo
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="message"></param>
        /// <param name="originalMessage"></param>
        /// <param name="stackTrace"></param>
        public ExcelAbnormalInfo(int row, int column, string message, string originalMessage, string stackTrace)
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
        private int Row { get; set; }
        /// <summary>
        /// 列坐标
        /// </summary>
        private int Column { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        private string Message { get; set; }
        /// <summary>
        /// 原始异常
        /// </summary>
        private string OriginalMessage { get; set; }
        /// <summary>
        /// 堆栈跟踪
        /// </summary>
        private string StackTrace { get; set; }
    }
    /// <summary>
    /// Excel异常
    /// </summary>
    public class ExcelException : Exception
    {
        public List<ExcelAbnormalInfo> ExcelExceptions { get; }

        public ExcelException(List<ExcelAbnormalInfo> excelExceptions)
        {
            ExcelExceptions = excelExceptions;
        }
    }
}
