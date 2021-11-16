using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApi.Base.IRepositories;
using WebApi.Models;
using WebApi.Models.Members;

namespace WebApi.Base.Repositories
{
    public class AdminMemberRepository : EfRepository<AdminMember>, IAdminMemberRepository
    {
        public AdminMemberRepository(EcShopContext context) : base(context)
        {

        }

        /// <summary>
        /// 使用Guid取得一筆管理員詳細資料
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        public async Task<AdminMember> GetDetailAsync(Expression<Func<AdminMember, bool>> whereLambda)
        {
            return await Context.Set<AdminMember>()
                .Include(q => q.AdminMemberStatus)
                .FirstOrDefaultAsync(whereLambda);
        }

        /// <summary>
        /// 取得所有管理員詳細資料
        /// </summary>
        /// <returns></returns>
        public IQueryable<AdminMember> GetDetailAll()
        {
            return Context.Set<AdminMember>()
                .Include(q => q.AdminMemberStatus)
                .AsQueryable();
        }
    }
}