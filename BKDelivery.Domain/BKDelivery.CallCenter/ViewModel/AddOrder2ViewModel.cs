﻿using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BKDelivery.Domain.Interfaces;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddOrder2ViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _saveCommand;
        private RelayCommand _addAddressCommand;
        private RelayCommand _addPackCommand;
        private RelayCommand _cleanupCommand;
        private Address _selectedHomeAddress;
        private Address _selectedDeliveryAddress;
        private Address _selectedInvokeAddress;
        private ObservableCollection<Package> _packsTypesCollecion;
        private ObservableCollection<Address> _addressesTypesCollecion;
        private ObservableCollection<Address> _invoiceAddressesCollection;
        private ObservableCollection<Address> _deliveryAddressesCollection;
        private int _packagesCount;

        public AddOrder2ViewModel(INavigationService navigationService, IDataService dataService,
            IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               PacksCollection = new ObservableCollection<Package>();
                               var result = await Task.Run(() => _dataService.PackagesByOrder(SelectedOrder.OrderId));
                               foreach (Package package in result)
                               {
                                   package.Category =
                                       await
                                           Task.Run(
                                               () => _dataService.Get<Category>(x => x.CategoryId == package.CategoryId));
                                   PacksCollection.Add(package);
                               }
                               PackagesCount = PacksCollection.Count;
                           }));
            }
        }


        private Order SelectedOrder => _navigationService.Parameter as Order;
        private Client SelectedClient => _dataService.Get<Client>(x => x.ClientId == SelectedOrder.ClientId);
        public KeyValuePair<TimeInterval, Courier> AvailableTimeInterval => _dataService.TimeIntervalFirstAvailable();
        //public KeyValuePair<TimeInterval, Courier> AvailableTimeInterval
        //{
        //    get
        //    {
        //        _dialogService.Show(Helpers.DialogType.Error, "couriers3.");
        //        return _dataService.TimeIntervalFirstAvailable();
        //    }
        //    set
        //    {
        //        try
        //        {
        //            Set(() => AvailableTimeInterval, ref _availableTimeInterval, value);
        //        }
        //        catch
        //        {
        //            _dialogService.Show(Helpers.DialogType.Error, "No couriers.");
        //        }
        //    }
        //}

        public ObservableCollection<Address> AddressesCollection
        {
            get
            {
                return new ObservableCollection<Address>(_dataService.AddressessByClient(
                    SelectedClient.ClientId, 1));
            }
            set { Set(() => AddressesCollection, ref _addressesTypesCollecion, value); }
        }

        public ObservableCollection<Address> DeliveryAddressesCollection
        {
            get
            {
                return
                    new ObservableCollection<Address>(_dataService.AddressessByClient(
                        SelectedClient.ClientId, 3));
            }
            set { Set(() => DeliveryAddressesCollection, ref _deliveryAddressesCollection, value); }
        }

        public ObservableCollection<Address> InvoiceAddressesCollection
        {
            get
            {
                return
                    new ObservableCollection<Address>(_dataService.AddressessByClient(
                        SelectedClient.ClientId, 2));
            }
            set { Set(() => InvoiceAddressesCollection, ref _invoiceAddressesCollection, value); }
        }

        public Address SelectedHomeAddress
        {
            get { return _selectedHomeAddress; }
            set { Set(() => SelectedHomeAddress, ref _selectedHomeAddress, value); }
        }

        public Address SelectedDeliveryAddress
        {
            get { return _selectedDeliveryAddress; }
            set { Set(() => SelectedDeliveryAddress, ref _selectedDeliveryAddress, value); }
        }

        public Address SelectedInvokeAddress
        {
            get { return _selectedInvokeAddress; }
            set { Set(() => SelectedInvokeAddress, ref _selectedInvokeAddress, value); }
        }

        public RelayCommand AddAddressCommand
        {
            get
            {
                return _addAddressCommand
                       ?? (_addAddressCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddAddressPageKey, SelectedOrder); }));
            }
        }

        public RelayCommand AddPackCommand
        {
            get
            {
                return _addPackCommand
                       ?? (_addPackCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddPackPageKey, SelectedOrder); }));
            }
        }

        public ObservableCollection<Package> PacksCollection
        {
            get { return _packsTypesCollecion; }
            set { Set(() => PacksCollection, ref _packsTypesCollecion, value); }
        }

        public int PackagesCount
        {
            get { return PacksCollection?.Count ?? 0; }
            set { Set(() => PackagesCount, ref _packagesCount, value); }
        }



        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               if (SelectedHomeAddress == null || SelectedDeliveryAddress == null ||
                                   SelectedInvokeAddress == null)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Choose one address from list for each type of addresses Home, Delivery and Invoice.");
                               }
                               else
                               {
                                   var order = _dataService.Get<Order>(x => x.OrderId == SelectedOrder.OrderId);
                                   order.FromAddressId = SelectedHomeAddress.AddressId;
                                   order.ToAddressId = SelectedDeliveryAddress.AddressId;
                                   order.TimeIntervalId = AvailableTimeInterval.Key.TimeIntervalId;
                                   order.CourierId = AvailableTimeInterval.Value.CourierId;
                                   order.InvoiceAddressId = SelectedInvokeAddress.AddressId;

                                   MessageBox.Show(PacksCollection[0].Category.Name);

                                   var interval = AvailableTimeInterval.Key;
                                   interval.IsTaken = true;
                                   _dataService.Update(interval);
                                   _dataService.Update(order);
                                   _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                               }
                           }));
            }
        }
    }
}