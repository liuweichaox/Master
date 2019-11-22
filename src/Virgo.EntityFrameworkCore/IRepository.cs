using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Virgo.EntityFrameworkCore
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Context"></typeparam>
    public interface IRepository<T, Context>
        where T : class
        where Context : DbContext
    {
        /// <summary>
        /// 数据库上下文实例
        /// </summary>
        Context DbContext { get; }

        /// <summary>
        /// 当前实体
        /// </summary>
        DbSet<T> Entities { get; }

        /// <summary>
        /// 当前实体
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// 通过主键ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(object id);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        void Insert(T entity, bool isSave = true);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        void Update(T entity, bool isSave = true);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        void Delete(T entity, bool isSave = true);
    }
}