using BKDelivery.Domain.Data.Repositories;

namespace BKDelivery.Domain.Data
{
    public interface IUnitOfWork
    {
        IAddressTypesRepository AddressTypesRepository { get; }
        IAddressesRepository AddressesRepository { get; }
        ICouriersRepository CouriersRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        IClientsRepository ClientsRepository { get; }
        IOrdersRepository OrdersRepository { get; }
        IPackagesRepository PackagesRepository { get; }
        ITimeIntervalsRepository TimeIntervalsRepository { get; }
        void SaveChanges();
    }
}