using System.Linq;
using System.Linq.Expressions;
using WebApi.Dtos.Products;
using WebApi.Models.Products;

namespace WebApi.Base.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// 使用Guid取得一筆包含關聯性資料的產品
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        Task<Product> GetDetailAsync(Expression<Func<Product, bool>> whereLambda);

        /// <summary>
        /// 取得所有包含關聯性資料的產品資料
        /// </summary>
        /// <returns></returns>
        IQueryable<Product> GetDetailAll();
    }
}