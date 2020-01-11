using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Virgo.Extensions
{
    /// <summary>
    /// Excel拓展
    /// </summary>
    public static class ExcelExtensions
    {
        /// <summary>
        /// 动态单元格转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> DynmaicCellToList<T>(this string[,] data) where T : class, new()
        {
            var cells = new List<T>();
            var propertyPosition = new Dictionary<int, string>();
            var type = typeof(T);
            var propertyInfos = type.GetProperties();
            for (int rowIndex = 0; rowIndex < data.GetLength(0); rowIndex++)
            {
                var rowInstance = Activator.CreateInstance<T>();
                for (int columnIndex = 0; columnIndex < data.GetLength(1); columnIndex++)
                {
                    var cell = data[rowIndex, columnIndex];
                    if (rowIndex == 0)
                    {
                        var propertyName = propertyInfos.SingleOrDefault(x => x.GetCustomAttribute<DescriptionAttribute>()?.Description == cell)?.Name;
                        if (propertyName != null)
                        {
                            propertyPosition.Add(columnIndex, propertyName);
                        }
                    }
                    else
                    {
                        var propertyName = propertyPosition.GetValueOrDefault(columnIndex);
                        if (propertyName != null)
                        {
                            var property = type.GetProperty(propertyName);
                            try
                            {
                                var value = Convert.ChangeType(cell, property.PropertyType);
                                property.SetValue(rowInstance, value);
                            }
                            catch
                            {
                                //自定义异常：单元格数据转换失败
                            }
                        }
                    }
                }
                if (rowIndex != 0)
                {
                    cells.Add(rowInstance);
                }
            }
            return cells;
        }
    }
}
