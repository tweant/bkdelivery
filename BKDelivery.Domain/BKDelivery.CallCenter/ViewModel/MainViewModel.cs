using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private IDataService _dataService;

        public MainViewModel(INavigationService navigationService,IDialogService dialogService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataService = dataService;
        }

        private RelayCommand ButtonCommand;
        private RelayCommand ButtonCommand1;
        private RelayCommand ButtonCommand2;
        private RelayCommand ButtonCommand3;
        private RelayCommand ButtonCommand4;
        private RelayCommand _cleanupCommand;

        /// <summary>
        /// Gets the AddressesButtonCommand.
        /// </summary>
        public RelayCommand CourierButtonCommand
        {
            get
            {
                return ButtonCommand
                       ?? (ButtonCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddCourierPageKey); }));
            }
        }

        public RelayCommand ClientButtonCommand
        {
            get
            {
                return ButtonCommand1
                       ?? (ButtonCommand1 = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddClientPageKey); }));
            }
        }

        public RelayCommand OrderButtonCommand
        {
            get
            {
                return ButtonCommand2
                       ?? (ButtonCommand2 = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey); }));
            }
        }

        public RelayCommand ClientsButtonCommand
        {
            get
            {
                return ButtonCommand3
                       ?? (ButtonCommand3 = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.ShowClientsPageKey); }));
            }
        }

        public RelayCommand OrdersButtonCommand
        {
            get
            {
                return ButtonCommand4
                       ?? (ButtonCommand4 = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.ShowOrdersPageKey); }));
            }
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               _dialogService.Show(Helpers.DialogType.BusyWaiting,"Connecting to \"268770.database.windows.net\" database. Please wait.");
                               await Task.Run(()=> _dataService.InitializeDataBase());
                               _dialogService.Hide();
                           }));
            }
        }
    }
}