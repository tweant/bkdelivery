namespace BKDelivery.Domain.Data
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        void SaveChanges();
    }
}
