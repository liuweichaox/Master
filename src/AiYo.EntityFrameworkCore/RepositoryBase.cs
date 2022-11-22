using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Virgo.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="IRepository{T, Context}"/>仓储抽象实现类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Context"></typeparam>
    public class RepositoryBase<T, Context> : IRepository<T, Context>
        where T : class
        where Context : DbContext
    {
        public RepositoryBase(Context _dbContext)
        {
            this.DbContext = _dbContext;
        }

        /// <summary>
        /// 数据库上下文实例
        /// </summary>
        public Context DbContext { get; }

        /// <summary>
        /// 当前实体
        /// </summary>
        public DbSet<T> Entities
        {
            get
            {
                return DbContext.Set<T>();
            }
        }

        /// <summary>
        /// 当前实体
        /// </summary>
        public IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isNeedSave"></param>
        public void Delete(T entity, bool isNeedSave = true)
        {
            Entities.Remove(entity);
            if (isNeedSave)
            {
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 通过主键ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isNeedSave"></param>
        public void Insert(T entity, bool isNeedSave = true)
        {
            Entities.Add(entity);
            if (isNeedSave)
            {
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isNeedSave"></param>
        public void Update(T entity, bool isNeedSave = true)
        {
            if (isNeedSave)
            {
                DbContext.SaveChanges();
            }
        }
    }
}