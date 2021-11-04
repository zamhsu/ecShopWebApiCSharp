using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Base.IRepositories;
using WebApi.Models;

namespace WebApi.Base.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly EcShopContext _context;

        public EfRepository(EcShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Database Context
        /// </summary>
        /// <value></value>
        public EcShopContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// 取得一筆資料
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> whereLambda)
        {
            return _context.Set<T>().FirstOrDefault(whereLambda);
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
        public virtual void Create(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Add(entity);
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
        public virtual void Create(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                foreach(var entity in entities)
                    _context.Add(entity);
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
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Update(entity);

                _context.SaveChanges();
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
        public virtual void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                foreach (var entity in entities)
                    _context.Update(entity);

                _context.SaveChanges();
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
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Remove(entity);

                _context.SaveChanges();
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
        public virtual void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException(nameof(entities));

                foreach (var entity in entities)
                    _context.Remove(entity);

                _context.SaveChanges();
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
        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}