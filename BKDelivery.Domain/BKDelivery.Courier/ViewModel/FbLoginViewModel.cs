using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using BKDelivery.Courier.Model;
using Facebook;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BKDelivery.Courier.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class FbLoginViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private RelayCommand _successLoginCommand;
        private RelayCommand _errorRetryCommand;
        private string _accessToken;
        private DateTime _tokenExpires;
        private string _grantedScopes;
        private string _deniedScopes;
        private string _error;
        private string _errorReason;
        private string _errorDescription;
        private RelayCommand _startLoadingPageCommand;
        private RelayCommand _stopLoadingPageCommand;
        private NotificationElement _loadingNotification;

        public FbLoginViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
        }


        public string AppID => "1335179443167660";
        public string Scopes => "public_profile,email,user_about_me";


        public string ErrorDescription
        {
            get { return _errorDescription; }
            set { Set(() => ErrorDescription, ref _errorDescription, value); }
        }

        public string ErrorReason
        {
            get { return _errorReason; }
            set { Set(() => ErrorReason, ref _errorReason, value); }
        }

        public string Error
        {
            get { return _error; }
            set { Set(() => Error, ref _error, value); }
        }

        public string DeniedScopes
        {
            get { return _deniedScopes; }
            set { Set(() => DeniedScopes, ref _deniedScopes, value); }
        }


        public string GrantedScopes
        {
            get { return _grantedScopes; }
            set { Set(() => GrantedScopes, ref _grantedScopes, value); }
        }


        public DateTime TokenExpires
        {
            get { return _tokenExpires; }
            set { Set(() => TokenExpires, ref _tokenExpires, value); }
        }

        public string AccessToken
        {
            get { return _accessToken; }
            set { Set(() => AccessToken, ref _accessToken, value); }
        }


        public RelayCommand ErrorRetryCommand
        {
            get
            {
                return _errorRetryCommand
                       ?? (_errorRetryCommand = new RelayCommand(
                           () =>
                           {
                               _dialogService.Show(Helpers.DialogType.Error,
                                   $"Error occured:\nError: {Error}\nError reason: {ErrorReason}\nError description: {ErrorDescription}");
                           }));
            }
        }

        public RelayCommand SuccessLoginCommand
        {
            get
            {
                return _successLoginCommand
                       ?? (_successLoginCommand = new RelayCommand(
                           async () =>
                           {
                               //_dialogService.Show(Helpers.DialogType.Success,
                               //    $"User logged in succesfully.\nToken expires: {TokenExpires}\nGranted scopes: {GrantedScopes}\nDenied scopes: {DeniedScopes}");

                               UserSession userSesssion =
                                   (Application.Current.Resources["Locator"] as ViewModelLocator).Session;

                               _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                               userSesssion.AccessToken = AccessToken;
                               userSesssion.DeniedScopes = DeniedScopes;
                               userSesssion.GrantedScopes = GrantedScopes;
                               userSesssion.IsSessionActive = true;
                               userSesssion.TokenExpires = TokenExpires;

                               var not = _dialogService.Show(Helpers.DialogType.BusyWaiting, "Loading user data.");
                               MainViewModel mainViewModel =
                                   (Application.Current.Resources["Locator"] as ViewModelLocator).Main;

                               FacebookClient fbClient = new FacebookClient(userSesssion.AccessToken);
                               var result =
                                   await fbClient.GetTaskAsync("https://graph.facebook.com/v2.8/me?fields=name");

                               JObject o = JObject.Parse(result.ToString());
                               mainViewModel.IsLoggedIn = true;
                               mainViewModel.LogInOutString = "Logout";
                               mainViewModel.UserProfileName = o.GetValue("name").ToString();
                               userSesssion.UserId = double.Parse(o.GetValue("id").ToString());

                               result = await fbClient.GetTaskAsync("https://graph.facebook.com/v2.8/me?fields=picture");
                               o = JObject.Parse(result.ToString());
                               mainViewModel.UserProfilePhotoString = o.SelectToken("$.picture.data.url").ToString();


                               not.Hide();
                               _dialogService.Show(Helpers.DialogType.Success,
                                   $"Logged in as {mainViewModel.UserProfileName}");
                           }));
            }
        }


        public RelayCommand StartLoadingPageCommand
        {
            get
            {
                return _startLoadingPageCommand
                       ?? (_startLoadingPageCommand = new RelayCommand(
                           () =>
                           {
                               _loadingNotification = _dialogService.Show(Helpers.DialogType.BusyWaiting,
                                   "Loading Facebook Authentication Page.\nPlease wait.");
                           }));
            }
        }


        public RelayCommand StopLoadingPageCommand
        {
            get
            {
                return _stopLoadingPageCommand
                       ?? (_stopLoadingPageCommand = new RelayCommand(
                           () => { _loadingNotification.Hide(); }));
            }
        }

        private string _webBrowserAddress = "http://google.pl";

        public string WebBrowserAddress
        {
            get { return _webBrowserAddress; }
            set { Set(() => WebBrowserAddress, ref _webBrowserAddress, value); }
        }
    }
}