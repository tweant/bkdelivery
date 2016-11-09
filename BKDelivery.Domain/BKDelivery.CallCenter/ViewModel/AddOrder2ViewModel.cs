using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddOrder2ViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        private RelayCommand _saveCommand;
        private RelayCommand _addAddressCommand;
        private RelayCommand _addPackCommand;
        private Address _selectedHomeAddress;
        private Address _selectedDeliveryAddress;
        private Address _selectedInvokeAddress;
        private ObservableCollection<Package> _packsTypesCollecion;
        private ObservableCollection<Address> _addressesTypesCollecion;
        private ObservableCollection<Address> _invoiceAddressesCollection;
        private ObservableCollection<Address> _deliveryAddressesCollection;

        public AddOrder2ViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        private Client SelectedClient => SelectedOrder.Client;
        private Order SelectedOrder => _navigationService.Parameter as Order;

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
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.AddAddressPageKey, SelectedOrder);
                           }));
            }
        }

        public RelayCommand AddPackCommand
        {
            get
            {
                return _addPackCommand
                       ?? (_addPackCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddPackPageKey); }));
            }
        }

        public ObservableCollection<Package> PacksCollection
        {
            get
            {
                return new ObservableCollection<Package>();
                //TODO Tutaj nie łączymy się z bazą, nie mamy jeszcze stworzonego Order - nie moge odwołać się do order ktore nie istnieje jeszcze
                //  return new ObservableCollection<Package>(_dataService.PackagesByOrder(#currentorderid));
            }
            set { Set(() => PacksCollection, ref _packsTypesCollecion, value); }
        }


        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               //_dataService.InitializeTransaction();
                               //var orderRepo = _dataService.UnitOfWork.Repository<Order>();
                               //var order = new Order
                               //{
                               //    FromAddress = SelectedHomeAddress,
                               //    ToAddress = SelectedDeliveryAddress,
                               //    //InvokeAddress = SelectedInvokeAddress,
                               //    //Client = _selectedClient,
                               //    //Packages = ,
                               //};
                               //orderRepo.Add(order);
                               //_dataService.SaveChanges();
                               //_navigationService.NavigateTo(ViewModelLocator.AddressesPageKey);
                           }));
            }
        }
    }
}