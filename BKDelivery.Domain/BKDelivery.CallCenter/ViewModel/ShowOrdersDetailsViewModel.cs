using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class ShowOrdersDetailsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        private RelayCommand _backCommand;
        private Order SelectedOrder => _navigationService.Parameter as Order;
        private ObservableCollection<Address> _addressesTypesCollecion;

        public ObservableCollection<Address> AddressesCollection;


        public ShowOrdersDetailsViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            AddressesCollection.Add(SelectedOrder.FromAddress);
            AddressesCollection.Add(SelectedOrder.ToAddress);
            AddressesCollection.Add(SelectedOrder.InvoiceAddress);
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
