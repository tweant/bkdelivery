using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BKDelivery.Domain.Interfaces;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddOrderViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _chooseclient;
        private RelayCommand _cleanupCommand;
        private ObservableCollection<Client> _clientsTypesCollecion;

        public AddOrderViewModel(INavigationService navigationService, IDataService dataService,
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
                               _dialogService.Show(Helpers.DialogType.BusyWaiting, "Please wait. Loading clients.");
                               var result = await Task.Run(() => _dataService.GetAll<Client>());
                               ClientsCollection = new ObservableCollection<Client>(result);
                               _dialogService.Hide();
                           }));
            }
        }

        public ObservableCollection<Client> ClientsCollection
        {
            get { return _clientsTypesCollecion; }
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
                               async () =>
                               {
                                   if (SelectedClient == null)
                                   {
                                       _dialogService.Show(Helpers.DialogType.Error,
                                           "Select client.");
                                   }
                                   else
                                   {
                                       var order = new Order
                                       {
                                           ClientId = _selectedClient.ClientId,
                                       };

                                       await Task.Run(() => _dataService.Add(order));
                                       _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2,
                                           order);
                                   }
                               }))
                        ;
                }
            }
        }
    }
}