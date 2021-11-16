using System.Linq.Expressions;
using WebApi.Models.Members;

namespace WebApi.Base.IRepositories
{
    public interface IAdminMemberRepository : IRepository<AdminMember>
    {
        /// <summary>
        /// 使用Guid取得一筆管理員詳細資料
        /// </summary>
        /// <param name="whereLambda">Lambda</param>
        /// <returns></returns>
        Task<AdminMember> GetDetailAsync(Expression<Func<AdminMember, bool>> whereLambda);

        /// <summary>
        /// 取得所有管理員詳細資料
        /// </summary>
        /// <returns></returns>
        IQueryable<AdminMember> GetDetailAll();
    }
}