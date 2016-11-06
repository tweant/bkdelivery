using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BKDelivery.Domain.Model;

namespace BKDelivery.Domain.Data
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly BkDeliveryContext _db;
        private readonly IDbSet<T> _objectSet;

        public GenericRepository(BkDeliveryContext db)
        {
            _db = db;
            _objectSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            _objectSet.Add(entity);
        }
        public void Delete(T entity)
        {
            _objectSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public T GetDetail(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.First(predicate);
        }

        public IEnumerable<T> GetOverview(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return _objectSet.Where(predicate);
            return _objectSet.AsEnumerable();
        }
    }

}
