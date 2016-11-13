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
        private Order SelectedOrder => _navigationService.Parameter as Order;

        public ObservableCollection<Address> AddressesCollection;

        public ShowOrdersDetailsViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private RelayCommand _cleanupCommand;
        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               _dialogService.Show(Helpers.DialogType.BusyWaiting, "Please wait. Loading order details.");
                               AddressesCollection = new ObservableCollection<Address>();
                               Address result = await Task.Run(() => _dataService.Get<Address>(x => x.AddressId == SelectedOrder.FromAddressId));
                               AddressesCollection.Add(result);
                               Address result1 = await Task.Run(() => _dataService.Get<Address>(x => x.AddressId == SelectedOrder.ToAddressId));
                               AddressesCollection.Add(result1);
                               Address result2 = await Task.Run(() => _dataService.Get<Address>(x => x.AddressId == SelectedOrder.InvoiceAddressId));
                               AddressesCollection.Add(result2);
                               _dialogService.Hide();
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
    }
}
