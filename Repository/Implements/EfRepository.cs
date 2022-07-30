using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Repository.Interfaces;

namespace Repository.Implements
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public EfRepository(DbContext context)
        {
            _context = context;
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

                await _context.AddAsync(entity);
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

                await _context.AddRangeAsync(entities);
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

                _context.UpdateRange(entities);
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

                _context.RemoveRange(entities);
            }
            catch
            {
                throw;
            }
        }
    }
}