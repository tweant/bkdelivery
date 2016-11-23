using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using GalaSoft.MvvmLight;
using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private RelayCommand _timetableNavigationCommand;
        private RelayCommand _routeNavigationCommand;
        private RelayCommand _ordersNavigationCommand;
        private RelayCommand _timeintervalsNavigationCommand;
        private ObservableCollection<UIElement> _notificationsCollection;

        //User
        private string _userProfilePhotoString = "/Images/defaultUser50.png";
        private string _userProfileName = "It's pretty alone down here.";
        private RelayCommand _loginoutCommand;
        private bool _isLoggedIn;
        private string _loginoutString = "Login";


        public MainViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            NotificationsCollection = new ObservableCollection<UIElement>();
        }

        public RelayCommand TimeIntervalsNavigationCommand
        {
            get
            {
                return _timeintervalsNavigationCommand
                       ?? (_timeintervalsNavigationCommand = new RelayCommand(
                           () =>
                           {
                               if (!IsLoggedIn)
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Please log in first to access that data.");
                               else
                                   _last.Hide();
                           }));
            }
        }

        public RelayCommand OrdersNavigationCommand
        {
            get
            {
                return _ordersNavigationCommand
                       ?? (_ordersNavigationCommand = new RelayCommand(
                           () =>
                           {
                               if (!IsLoggedIn)
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Please log in first to access that data.");
                               else
                                   _last = _dialogService.Show(Helpers.DialogType.Success,
                                       "Congratulations!");
                           }));
            }
        }

        public RelayCommand RouteNavigationCommand
        {
            get
            {
                return _routeNavigationCommand
                       ?? (_routeNavigationCommand = new RelayCommand(
                           () =>
                           {
                               if (!IsLoggedIn)
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Please log in first to access that data.");
                               else
                                   _lastbusy.Hide();
                           }));
            }
        }

        public RelayCommand TimetableNavigationCommand
        {
            get
            {
                return _timetableNavigationCommand
                       ?? (_timetableNavigationCommand = new RelayCommand(
                           () =>
                           {
                               if (!IsLoggedIn)
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Please log in first to access that data.");
                               else
                                   _lastbusy = _dialogService.Show(Helpers.DialogType.BusyWaiting,
                                       "Connecting to \"268770.database.windows.net\" database. Please wait.");
                           }));
            }
        }

        private NotificationElement _lastbusy;
        private NotificationElement _last;


        public ObservableCollection<UIElement> NotificationsCollection
        {
            get { return _notificationsCollection; }
            set { Set(() => NotificationsCollection, ref _notificationsCollection, value); }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(() => IsLoggedIn, ref _isLoggedIn, value); }
        }

        public string UserProfilePhotoString
        {
            get { return _userProfilePhotoString; }
            set { Set(() => UserProfilePhotoString, ref _userProfilePhotoString, value); }
        }

        public string UserProfileName
        {
            get { return _userProfileName; }
            set { Set(() => UserProfileName, ref _userProfileName, value); }
        }

        public RelayCommand LogInOutCommand
            => _loginoutCommand ?? (_loginoutCommand = new RelayCommand(ExecuteLogInOutCommand));


        private void ExecuteLogInOutCommand()
        {
            if (IsLoggedIn)
            {
                IsLoggedIn = false;
                UserProfileName = "It's pretty alone down here.";
                UserProfilePhotoString = "/Images/defaultUser50.png";
                LogInOutString = "Login";
                UserSession userSesssion =
                    (Application.Current.Resources["Locator"] as ViewModelLocator).Session;

                _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                userSesssion.AccessToken = null;
                userSesssion.DeniedScopes = null;
                userSesssion.GrantedScopes = null;
                userSesssion.IsSessionActive = true;
                userSesssion.TokenExpires = DateTime.Now;
                userSesssion.IsSessionActive = false;

                _navigationService.NavigateTo(ViewModelLocator.StartUpPageKey);
                _dialogService.Show(Helpers.DialogType.Success, "Succesfully logged out.");
            }
            else
            {
                //_dialogService.Show(Helpers.DialogType.Success, "Login");
                FbLoginViewModel _viewModel = (Application.Current.Resources["Locator"] as ViewModelLocator).FbLogin;
                _viewModel.WebBrowserAddress =
                   (string.Format(
                       "http://www.google.pl"));
                _navigationService.NavigateTo(ViewModelLocator.FacebookLogInPageKey);
                
                //string p_scopes = "public_profile,email,user_about_me";
                string returnURL = WebUtility.UrlEncode("https://www.facebook.com/connect/login_success.html");
                string scopes = WebUtility.UrlEncode(_viewModel.Scopes);
                _viewModel.StartLoadingPageCommand.Execute(null);
                _viewModel.WebBrowserAddress =
                    (string.Format(
                        "https://www.facebook.com/v2.8/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token%2Cgranted_scopes&scope={2}&display=popup",
                        new object[] {_viewModel.AppID, returnURL, scopes}));
            }
        }


        public string LogInOutString
        {
            get { return _loginoutString; }
            set { Set(() => LogInOutString, ref _loginoutString, value); }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}