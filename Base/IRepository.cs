using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Base
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 取得一筆資料
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// 取得所有資料(無法被EF追蹤且資料為唯讀)
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllNoTracking();

        /// <summary>
        /// 新增一筆資料
        /// </summary>
        /// <param name="entity">Entity</param>
        void Create(T entity);

        /// <summary>
        /// 新增多筆資料
        /// </summary>
        /// <param name="entities">Entities</param>
        void Create(IEnumerable<T> entities);

        /// <summary>
        /// 更新一筆資料
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// 更新多筆資料
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// 刪除一筆資料
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}