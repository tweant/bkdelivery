using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class ShowOrdersViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        private RelayCommand _addAddressCommand;
        private ObservableCollection<Order> _ordersCollection;

        public ShowOrdersViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        public ObservableCollection<Order> OrdersCollection
        {
            get { return new ObservableCollection<Order>(_dataService.OrdersAll()); }
            set { Set(() => OrdersCollection, ref _ordersCollection, value); }
        }

        public RelayCommand OrdersButtonCommand
        {
            get
            {
                return _addAddressCommand
                       ?? (_addAddressCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddAddressPageKey); }));
            }
        }
    }
}