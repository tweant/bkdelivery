﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Domain.Model;

namespace BKDelivery.CallCenter.Model
{
    public interface IDataService
    {
        IEnumerable<AddressType> AddressTypesAll();

        void AddressAdd(Address address);
        IEnumerable<Address> AddressessByClient(int clientId);
        IEnumerable<Address> AddressessByClient(int clientId, int addressTypeId);

        void CourierAdd(Courier courier);

        void ClientAdd(Client client);
        IEnumerable<Client> ClientsAll();

        IEnumerable<Category> CategoriesAll();

        void PackageAdd(Package package, Order order);
        IEnumerable<Package> PackagesByOrder(int orderId);

        void OrderAdd(Order order);
        IEnumerable<Order> OrdersAll();
    }
}
