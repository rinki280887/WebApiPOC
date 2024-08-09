using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPOC.DataBaseModel
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        #region Fields
        internal transactionDBContext _context;
        private bool _isDisposed;
        #endregion

        #region Properties
        private DbSet<TEntity> _entities { get; set; }
        #endregion

        public Repository(transactionDBContext context)
        {
            _isDisposed = false;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return  entity;
        }

        public TEntity Delete(int id)
        {
            TEntity entity = GetById(id);
            _entities.Remove(entity);
            _context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id)
        {
            TEntity entity = await GetByIdAsync(id).ConfigureAwait(false);
            _entities.Remove(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public TEntity GetById(int id)
        {
           return _entities.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id).ConfigureAwait(false); 
        }

        public TEntity Update(TEntity entity)
        {
            _entities.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _isDisposed = true;
        }
    }
}
