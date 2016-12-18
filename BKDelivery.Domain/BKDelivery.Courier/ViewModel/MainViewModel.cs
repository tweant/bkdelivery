using System;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

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


        private async void ExecuteLogInOutCommand()
        {
            if (AzureAdService.IsLoggedIn())
            {
                AzureAdService.Logout();
                IsLoggedIn = false;
                UserProfileName = "It's pretty alone down here.";
                UserProfilePhotoString = "/Images/defaultUser50.png";
                LogInOutString = "Login";

                _navigationService.NavigateTo(ViewModelLocator.StartUpPageKey);
                _dialogService.Show(Helpers.DialogType.Success, "Succesfully logged out.");
            }
            else
            {
                await AzureAdService.Login();
                if (AzureAdService.IsLoggedIn())
                {
                    IsLoggedIn = true;
                    LogInOutString = "Logout";

                    User signedInUser = new User();
                    try
                    {
                        signedInUser = (User) await AzureAdService._client.Me.ExecuteAsync();
                        UserProfileName = signedInUser.DisplayName;
                    }
                    catch (Exception e)
                    {
                        _dialogService.Show(Helpers.DialogType.Error,
                            $"Error getting signed in user {AzureAdService.ExtractErrorMessage(e)}");
                    }

                    if (signedInUser.ObjectId != null)
                    {
                        IUser sUser = (IUser) signedInUser;
                        try
                        {

                            //
                            // Download the thumbnailphoto.
                            // if no value in the attribute, then you will need to handle the exception.
                            //

                            using (var dssr = await sUser.ThumbnailPhoto.DownloadAsync())
                            {
                                var stream = dssr.Stream;
                                byte[] buffer = null;
                                long bCount = 0;
                                if (stream.CanSeek)
                                {
                                    buffer = new byte[stream.Length];
                                    bCount = stream.Length;
                                }
                                else
                                {
                                    buffer = new byte[long.Parse(dssr.Headers["Content-Length"])];
                                    bCount = long.Parse(dssr.Headers["Content-Length"]);
                                }
                                await stream.ReadAsync(buffer, 0, (int) bCount);

                                string pickPath = "Images/Users/userphoto.png";
                                Directory.CreateDirectory(Path.GetDirectoryName(pickPath));
                                FileStream fs = new FileStream(pickPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                await fs.WriteAsync(buffer, 0, (int) bCount);
                                UserProfilePhotoString = @"pack://siteoforigin:,,,/Images/Users/userphoto.png";
                                _dialogService.Show(Helpers.DialogType.Success, UserProfilePhotoString);
                            }
                        }
                        catch (Exception e)
                        {
                            if (e.InnerException != null)
                            {
                                string sEx = e.InnerException.GetType().ToString();
                                if (sEx == "Microsoft.Data.OData.ODataErrorException")
                                {
                                    //
                                    // Handle exception.
                                }
                            }
                            else
                            {
                                _dialogService.Show(Helpers.DialogType.Error,
                                    $"Error getting signed in user {AzureAdService.ExtractErrorMessage(e)}");
                            }
                        }
                    }
                }
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