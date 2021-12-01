using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Base.IRepositories;
using WebApi.Models;

namespace WebApi.Base.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Database Context
        /// </summary>
        /// <value></value>
        public DbContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// 取得一筆資料
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(whereLambda);
        }

        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        /// <summary>
        /// 取得所有資料(無法被EF追蹤且資料為唯讀)
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAllNoTracking()
        {
            return _context.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// 新增一筆資料
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task CreateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Add(entity);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 新增多筆資料
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task CreateAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                foreach(var entity in entities)
                    _context.Add(entity);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 更新一筆資料
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Update(entity);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 更新多筆資料
        /// </summary>
        /// /// <param name="entities">Entities</param>
        public virtual async Task UpdateAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                foreach (var entity in entities)
                    _context.Update(entity);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除一筆資料
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Remove(entity);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除多筆資料
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task DeleteAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                foreach (var entity in entities)
                    _context.Remove(entity);

                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}