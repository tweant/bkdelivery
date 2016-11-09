using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddOrderViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        private RelayCommand _chooseclient;
        private ObservableCollection<Client> _clientsTypesCollecion;

        public AddOrderViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        public ObservableCollection<Client> ClientsCollection
        {
            get { return new ObservableCollection<Client>(_dataService.ClientsAll()); }
            set { Set(() => ClientsCollection, ref _clientsTypesCollecion, value); }
        }


        private Client _selectedClient;

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { Set(() => SelectedClient, ref _selectedClient, value); }
        }

        public RelayCommand ChooseClient
        {
            get
            {
                {
                    return _chooseclient
                           ?? (_chooseclient = new RelayCommand(
                               () =>
                               {
                                   var order = new Order
                                   {
                                       ClientId = _selectedClient.ClientId,
                                   };
                                   _dataService.OrderAdd(order);
                                   _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2, order);
                               }));
                }
            }
        }
    }
}