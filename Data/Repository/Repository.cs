using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DataContext _context;
        internal DbSet<TEntity> _dbSet;

        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if(query != null && filter != null)
            {
                query.Where(filter);
            }

            if(orderby != null)
            {
                return orderby(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            return _dbSet.Any(filter);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            return _dbSet.FirstOrDefault(filter);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            return _dbSet.SingleOrDefault(filter);
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            return _dbSet.Count(filter);
        }
    }
}
