using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// 非同步儲存變更
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch
            {

                throw;
            }
        }
    }
}