namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 特定Entity的Repositry
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <returns></returns>
        IRepository<T> Repository<T>() where T : class;

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