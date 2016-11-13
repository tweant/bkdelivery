using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BKDelivery.Domain.Interfaces
{
    public interface IDbContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        DbEntityEntry Entry(object entity);
    }
}
