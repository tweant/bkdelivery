using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class ShowClientsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        private ObservableCollection<Client> _clientsCollection;

        private RelayCommand _addAddressCommand;

        public ShowClientsViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
        }

        public ObservableCollection<Client> ClientsCollection
        {
            get { return new ObservableCollection<Client>(_dataService.ClientsAll()); }
            set { Set(() => ClientsCollection, ref _clientsCollection, value); }
        }

        public RelayCommand ClientsButtonCommand
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