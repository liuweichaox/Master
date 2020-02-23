using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Virgo.Extensions
{
    /// <summary>
    /// AutoMapper映射拓展
    /// </summary>
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// 将源类型对象转换为目标类型对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">原类型对象</param>
        /// <returns>目标类型对象</returns>
        public static TDestination Map<TSource, TDestination>(this TSource source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        /// 将源类型对象转换为目标类型对象集合
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源类型对象集合</param>
        /// <returns>目标类型对象集合</returns>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<List<TSource>, List<TDestination>>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }
    }
}

