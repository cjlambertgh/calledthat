using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int Count(Expression<Func<TEntity, bool>> filter = null);
        void Add(TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> filter = null);
        void Delete(TEntity entity);
        void Delete(object id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null);
        TEntity GetById(object id);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> filter = null);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> All();
    }
}