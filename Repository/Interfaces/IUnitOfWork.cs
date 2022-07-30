namespace Repository.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 非同步儲存變更
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}