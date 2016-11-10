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
        private readonly IDialogService _dialogService;

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
        private int _packagesCount;

        public AddOrder2ViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private Client SelectedClient => SelectedOrder.Client;
        private Order SelectedOrder => _navigationService.Parameter as Order;
        public KeyValuePair<TimeInterval, Courier> AvailableTimeInterval => _dataService.TimeIntervalFirstAvailable();

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
            get { return new ObservableCollection<Package>(_dataService.PackagesByOrder(SelectedOrder.OrderId)); }
            set { Set(() => PacksCollection, ref _packsTypesCollecion, value); }
        }

        public int PackagesCount
        {
            get { return PacksCollection.Count; }
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
                                   SelectedOrder.FromAddressId = SelectedHomeAddress.AddressId;
                                   SelectedOrder.ToAddressId = SelectedDeliveryAddress.AddressId;
                                   SelectedOrder.TimeIntervalId = AvailableTimeInterval.Key.TimeIntervalId;
                                   SelectedOrder.CourierId = AvailableTimeInterval.Value.CourierId;
                                   SelectedOrder.InvoiceAddressId = SelectedInvokeAddress.AddressId;
                                   _dataService.OrderEdit(SelectedOrder);
                                   AvailableTimeInterval.Key.IsTaken = true;
                                   _dataService.TimeIntervalEdit(AvailableTimeInterval.Key);
                                   _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                               }
                           }));
            }
        }
    }
}