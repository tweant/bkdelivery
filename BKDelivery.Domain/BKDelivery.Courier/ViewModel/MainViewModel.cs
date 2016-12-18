using System;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
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
        private ImageSource _userProfilePhotoString;
        private string _userProfileName = "It's pretty alone down here.";
        private RelayCommand _loginoutCommand;
        private bool _isLoggedIn;
        private string _loginoutString = "Login";

        //
        private BitmapImage _defaultUserPhoto;


        public MainViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            NotificationsCollection = new ObservableCollection<UIElement>();

            _defaultUserPhoto = new BitmapImage(new Uri("/Images/defaultUser50.png", UriKind.Relative));
            UserProfilePhotoString = _defaultUserPhoto;
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

        public ImageSource UserProfilePhotoString
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
                UserProfilePhotoString = _defaultUserPhoto;
                LogInOutString = "Login";

                _navigationService.NavigateTo(ViewModelLocator.StartUpPageKey);
                _dialogService.Show(Helpers.DialogType.Success, "Succesfully logged out.");
            }
            else
            {
                try
                {
                    await AzureAdService.Login();
                }
                catch (Exception e)
                {
                    AzureAdService.HandleException(e);
                }

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
                            using (var dssr = await sUser.ThumbnailPhoto.DownloadAsync())
                            {

                                BitmapImage bitmapimage = new BitmapImage();
                                bitmapimage.BeginInit();
                                bitmapimage.StreamSource = dssr.Stream;
                                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapimage.EndInit();
                                UserProfilePhotoString = bitmapimage;

                            }

                        }
                        catch (Exception e)
                        {
                            if (e.Message.Contains("Request_ResourceNotFound") || (e.InnerException!=null && e.InnerException.Message.Contains("Request_ResourceNotFound")))
                            {
                                    //User doesn't have profile picture
                                    return;
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