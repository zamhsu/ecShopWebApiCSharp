using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Base.IRepositories;
using WebApi.Dtos.Products;
using WebApi.Models;
using WebApi.Models.Products;

namespace WebApi.Base.Repositories
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {

        }

        /// <summary>
        /// 使用Guid取得一筆產品詳細資料
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        public async Task<Product> GetDetailAsync(Expression<Func<Product, bool>> whereLambda)
        {
            return await Context.Set<Product>()
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus)
                .FirstOrDefaultAsync(whereLambda);
        }

        /// <summary>
        /// 取得所有產品詳細資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> GetDetailAll()
        {
            return Context.Set<Product>()
                .Include(q => q.ProductCategoryType)
                .Include(q => q.ProductUnitType)
                .Include(q => q.ProductStatus)
                .AsQueryable();
        }
    }
}