using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        public MainViewModel(INavigationService navigationService, IDialogService dialogService,
            IDataService dataService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataService = dataService;
            _notificationsCollection = new ObservableCollection<UIElement>();

            //Azure
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
        }

        #region Notifications

        private ObservableCollection<UIElement> _notificationsCollection;

        public ObservableCollection<UIElement> NotificationsCollection
        {
            get { return _notificationsCollection; }
            set { Set(() => NotificationsCollection, ref _notificationsCollection, value); }
        }

        #endregion

        private RelayCommand ButtonCommand;
        private RelayCommand ButtonCommand1;
        private RelayCommand ButtonCommand2;
        private RelayCommand ButtonCommand3;
        private RelayCommand ButtonCommand4;
        private RelayCommand _cleanupCommand;
        private RelayCommand _azureAdLoginCommand;
        private string _azureAdLoginButtonContext = "Azure AD Login";

        /// <summary>
        /// Gets the AddressesButtonCommand.
        /// </summary>
        public RelayCommand CourierButtonCommand
        {
            get
            {
                return ButtonCommand
                       ?? (ButtonCommand = new RelayCommand(
                           () =>
                           {
                               if (AzureAdService.IsLoggedIn())
                                   _navigationService.NavigateTo(ViewModelLocator.AddCourierPageKey);
                               else
                               {
                                   _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Log in to Azure Active Directory first to access that data.");
                               }
                           }));
            }
        }

        public string AzureAdLoginButtonContext
        {
            get { return _azureAdLoginButtonContext; }
            set { Set(() => AzureAdLoginButtonContext, ref _azureAdLoginButtonContext, value); }
        }

        public RelayCommand AzureAdLoginCommand => _azureAdLoginCommand
                                                   ?? (_azureAdLoginCommand = new RelayCommand(ExecuteMyCommand));

        private async void ExecuteMyCommand()
        {
            if (AzureAdService.IsLoggedIn())
            {
                AzureAdService.Logout();
                AzureAdLoginButtonContext = "Azure AD Login";
                _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
            }
            else
            {
                await AzureAdService.Login();
                if(AzureAdService.IsLoggedIn())
                    AzureAdLoginButtonContext = "Azure AD Logout";
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
                               var dialog = _dialogService.Show(Helpers.DialogType.BusyWaiting,
                                   "Connecting to \"bkdelivery.database.windows.net\" database. Please wait.");
                               await Task.Run(() => _dataService.InitializeDataBase());
                               _dialogService.Hide(dialog);
                           }));
            }
        }
    }
}