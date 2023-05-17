using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IRepository<TEntity>: IDisposable
    {
        void Remove(TEntity row);
        void RemoveRange(List<TEntity> rows);
        void AddRange(List<TEntity> rows);
        void Add(TEntity row);
        void SaveChanges();
        List<TResult> GetMany<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            bool distinct = false
        );
        List<TEntity> GetMany(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = false
        );
        TEntity? FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = true,
            params string[] includeProperties
        );
        TResult? FirstOrDefault<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select
        );
        bool Exists(Expression<Func<TEntity, bool>> predicate);
    }

    public abstract class RepositoryBase<TEntity>: IRepository<TEntity>
        where TEntity : class
    {
        protected readonly FilesMonitorDbContext _db;
        protected readonly DbSet<TEntity> _set;
        private bool disposedValue;

        public RepositoryBase(FilesMonitorDbContext db)
        {
            _db = db;
            _set = _db.Set<TEntity>();
        }

        public void Remove(TEntity row)
            => _set.Remove(row);

        public void RemoveRange(List<TEntity> rows)
            => _set.RemoveRange(rows);

        public void AddRange(List<TEntity> rows)
            => _set.AddRange(rows);

        public void Add(TEntity row)
            => _set.Add(row);

        public void SaveChanges()
            => _db.SaveChanges();

        public List<TResult> GetMany<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            bool distinct = false
        )
        {
            var query = _set.Where(predicate).Select(select);

            if (distinct)
                query = query.Distinct();

            return query.ToList();
        }

        public List<TEntity> GetMany(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = false
        )
        {
            var query = _set.Where(predicate);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.ToList();
        }

        public TEntity? FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = true,
            params string[] includeProperties
        )
        {
            var query = _set.AsQueryable();

            foreach (var property in includeProperties)
                query = query.Include(property);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault(predicate);
        }

        public TResult? FirstOrDefault<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select
        )
        {
            return _set.Where(predicate).Select(select).FirstOrDefault();
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
            => _set.Any(predicate);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
