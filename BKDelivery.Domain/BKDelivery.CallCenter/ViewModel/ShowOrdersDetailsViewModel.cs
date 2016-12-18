using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace BKDelivery.CallCenter.ViewModel
{
    public class ShowOrdersDetailsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _backCommand;
        private RelayCommand _editCommand;
        private ObservableCollection<Address> _addressesCollection;

        private Order SelectedOrder => _navigationService.Parameter as Order;


        public ObservableCollection<Address> AddressesCollection
        {
            get { return _addressesCollection; }
            set { Set(() => AddressesCollection, ref _addressesCollection, value); }
        }
        public ObservableCollection<Package> PacksCollection
        {
            get { return _packsTypesCollecion; }
            set { Set(() => PacksCollection, ref _packsTypesCollecion, value); }
        }
        public ObservableCollection<Client> ClientsCollection
        {
            get { return _clientsCollection; }
            set { Set(() => ClientsCollection, ref _clientsCollection, value); }
        }
        public ObservableCollection<Courier> CouriersCollection
        {
            get { return _couriersCollection; }
            set { Set(() => CouriersCollection, ref _couriersCollection, value); }
        }

        public TimeInterval Time
        {
            get { return _time; }
            set { Set(() => Time, ref _time, value); }
        }

        public ShowOrdersDetailsViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private RelayCommand _cleanupCommand;
        private ObservableCollection<Package> _packsTypesCollecion;
        private ObservableCollection<Client> _clientsCollection;
        private ObservableCollection<Courier> _couriersCollection;
        private TimeInterval _time;

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               var dialog = _dialogService.Show(Helpers.DialogType.BusyWaiting, "Please wait. Loading order details.");
                               Time = new TimeInterval();
                               Time = await Task.Run(() => _dataService.Get<TimeInterval>(x => x.TimeIntervalId == SelectedOrder.TimeIntervalId));
                               ClientsCollection = new ObservableCollection<Client>();
                               var result4 = await Task.Run(() => _dataService.SearchClientId(SelectedOrder.ClientId));
                               foreach (Client client in result4)
                               {
                                   ClientsCollection.Add(client);
                               }
                               CouriersCollection = new ObservableCollection<Courier>();
                               var result5 = await Task.Run(() => _dataService.SearchCourierId(SelectedOrder.CourierId));
                               foreach (Courier courier in result5)
                               {
                                   CouriersCollection.Add(courier);
                               }
                               AddressesCollection = new ObservableCollection<Address>();
                               var result = await Task.Run(() => _dataService.AddressesByOrder(SelectedOrder.FromAddressId));
                               foreach (Address address in result)
                               {
                                   address.AddressType = await Task.Run(() => _dataService.Get<AddressType>(x => x.AddressTypeId == address.AddressTypeId));
                                   AddressesCollection.Add(address);
                               }
                               var result1 = await Task.Run(() => _dataService.AddressesByOrder(SelectedOrder.ToAddressId));
                               foreach (Address address in result1)
                               {
                                   address.AddressType = await Task.Run(() => _dataService.Get<AddressType>(x => x.AddressTypeId == address.AddressTypeId));
                                   AddressesCollection.Add(address);
                               }
                               var result2 = await Task.Run(() => _dataService.AddressesByOrder(SelectedOrder.InvoiceAddressId));
                               foreach (Address address in result2)
                               {
                                   address.AddressType = await Task.Run(() => _dataService.Get<AddressType>(x => x.AddressTypeId == address.AddressTypeId));
                                   AddressesCollection.Add(address);
                               }
                               PacksCollection = new ObservableCollection<Package>();
                               var result3 = await Task.Run(() => _dataService.PackagesByOrder(SelectedOrder.OrderId));
                               foreach (Package package in result3)
                               {
                                   package.Category = await Task.Run(() => _dataService.Get<Category>(x => x.CategoryId == package.CategoryId));
                                   PacksCollection.Add(package);
                               }   
                               _dialogService.Hide(dialog);
                           }));
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                       ?? (_backCommand = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.ShowOrdersPageKey);
                           }));
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand
                       ?? (_editCommand = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2, SelectedOrder);
                           }));
            }
        }
    }
}
