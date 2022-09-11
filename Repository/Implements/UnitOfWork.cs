using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System.Collections;

namespace Repository.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private Hashtable _repositories;
        private bool _disposed = false;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories is null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new EfRepository<T>(_context);
            }

            return (IRepository<T>)_repositories[type];
        }

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// 非同步儲存變更
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 清除此Class的資源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清除此Class的資源。
        /// </summary>
        /// <param name="disposing">是否在清理中？</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }
    }
}