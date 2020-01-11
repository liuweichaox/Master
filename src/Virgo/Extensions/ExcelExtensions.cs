using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Virgo.Extensions
{
    /// <summary>
    /// Excel拓展
    /// </summary>
    public static class ExcelExtensions
    {
        /// <summary>
        /// 读取Excel工作表，将单元格数据转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static List<T> ReadWorksheet<T>(this Stream stream, int worksheet = 0) where T : class, new()
        {
            var cells = ReadWorksheet(stream, worksheet);
            return CellsToList<T>(cells);
        }
        /// <summary>
        /// 读取Excel工作表，将单元格数据转换为二维数组
        /// </summary>
        /// <param name="stream">Excel文件</param>
        /// <param name="worksheet">指定工作表。默认第一个</param>
        /// <returns></returns>
        public static string[,] ReadWorksheet(this Stream stream, int worksheet = 0)
        {
            using var pck = new ExcelPackage(stream);
            var ws = pck.Workbook.Worksheets[worksheet];
            var minColumnNum = ws.Dimension.Start.Column; //工作区开始列
            var maxColumnNum = ws.Dimension.End.Column; //工作区结束列
            var minRowNum = ws.Dimension.Start.Row; //工作区开始行号
            var maxRowNum = ws.Dimension.End.Row; //工作区结束行号
            string[,] cells = new string[maxRowNum, maxColumnNum];
            for (var row = minRowNum; row <= maxRowNum; row++)
            {
                for (int col = minColumnNum; col <= maxColumnNum; col++)
                {
                    cells[row, col] = ws.Cells[row, col].ToString();
                }
            }
            return cells;
        }

        /// <summary>
        /// 动态单元格转换
        /// </summary>
        /// <typeparam name="T">需要转换的类型</typeparam>
        /// <param name="cells">Excel单元格数据</param>
        /// <returns></returns>
        public static List<T> CellsToList<T>(this string[,] cells) where T : class, new()
        {
            var list = new List<T>();
            var propertyPosition = new Dictionary<int, string>();
            var propertyInfos = typeof(T).GetProperties();
            var errors = new List<ExcelExceptionTemplate>();
            for (int row = 0; row < cells.GetLength(0); row++)
            {
                var rowInstance = Activator.CreateInstance<T>();
                for (int col = 0; col < cells.GetLength(1); col++)
                {
                    var cell = cells[row, col];
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
    }

    /// <summary>
    /// Excel异常模板
    /// </summary>
    public class ExcelExceptionTemplate
    {
        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// 列
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
