using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
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


        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            NotificationsCollection = new ObservableCollection<UIElement>();
        }

        public RelayCommand TimeIntervalsNavigationCommand
        {
            get
            {
                return _timeintervalsNavigationCommand
                       ?? (_timeintervalsNavigationCommand = new RelayCommand(
                           () => {
                               if (!IsLoggedIn)
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Please log in first to access that data.");
                               else
                                   _last.Hide(); }));
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
                _dialogService.Show(Helpers.DialogType.Success, "Logout");
            else
                _dialogService.Show(Helpers.DialogType.Success, "Login");
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