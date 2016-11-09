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


        private Client _SelectedClient;

        public Client SelectedClient
        {
            get { return _SelectedClient; }
            set { Set(() => SelectedClient, ref _SelectedClient, value); }
        }

        public RelayCommand ChooseClient
        {
            get
            {
                if (SelectedClient != null)
                {
                    return _chooseclient
                           ?? (_chooseclient = new RelayCommand(
                               () => { _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2); }));
                }
                else
                {
                    return _chooseclient
                           ?? (_chooseclient = new RelayCommand(
                               () => { _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2); }));
                }
            }
        }
    }
}