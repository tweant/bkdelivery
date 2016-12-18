using System;
using System.Collections.ObjectModel;
using System.Linq;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using BKDelivery.CallCenter.Helpers;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddCourierViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _saveCommand;
        private RelayCommand _cleanupCommand;

        public AddCourierViewModel(INavigationService navigationService, IDataService dataService,
            IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private string _name;
        private string _surname;
        private int _phonenumber;
        private string _userPrincipalName;
        private bool _isUserValidated;

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public string Surname
        {
            get { return _surname; }
            set { Set(() => Surname, ref _surname, value); }
        }

        public int PhoneNumber
        {
            get { return _phonenumber; }
            set { Set(() => PhoneNumber, ref _phonenumber, value); }
        }

        public bool IsUserValidated
        {
            get { return _isUserValidated; }
            set { Set(() => IsUserValidated, ref _isUserValidated, value); }
        }

        public string UserPrincipalName
        {
            get { return _userPrincipalName; }
            set { Set(() => UserPrincipalName, ref _userPrincipalName, value); }
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               Name = string.Empty;
                               Surname = string.Empty;
                               PhoneNumber = 0;
                               IsUserValidated = false;
                           }));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           async () =>
                           {
                               if (PhoneNumber < 100000000 || PhoneNumber > 999999999)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Put correct phone number.");
                               }
                               else if (Name.Length == 0)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Empty name.");
                               }
                               else if (Surname.Length == 0)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Empty surname.");
                               }
                               else
                               {

                                   var dialog = _dialogService.Show(DialogType.BusyWaiting, "Validating user data");
                                   try
                                   {

                                       var defaultDomain =
                                           AzureAdService.TenantDetail.VerifiedDomains.First(
                                               x => x.@default.HasValue && x.@default.Value);

                                       UserPrincipalName = Strings.LettersOnly(Name.ToLower()) + "." +
                                                           Strings.LettersOnly(Surname.ToLower()) + "@" +
                                                           defaultDomain.Name;

                                       bool isAvailable = await
                                           AzureAdService.IsPrincipalNameAvailable(AzureAdService._client,
                                               UserPrincipalName);

                                       int counter = 1;

                                       while (!isAvailable)
                                       {
                                           UserPrincipalName = Strings.LettersOnly(Name.ToLower()) + "." +
                                                               Strings.LettersOnly(Surname.ToLower()) + counter + "@" +
                                                               defaultDomain.Name;
                                           counter++;
                                           isAvailable = await
                                               AzureAdService.IsPrincipalNameAvailable(AzureAdService._client,
                                                   UserPrincipalName);
                                       }

                                       Group group = await AzureAdService.FindGroupByObjectId(AzureAdService._client,
                                           AzureBkDeliveryConstants.GroupCouriersObjectId);

                                       var user = await
                                           AzureAdService.CreateNewUser(AzureAdService._client, defaultDomain, Name,
                                               Surname,
                                               PhoneNumber, UserPrincipalName);

                                       await AzureAdService.AddUserToGroup(group, user);



                                       IsUserValidated = true;
                                   }
                                   catch (Exception e)
                                   {
                                       _dialogService.Show(DialogType.Error,
                                           "Error: " + AzureAdService.ExtractErrorMessage(e));
                                   }
                                   finally
                                   {
                                       dialog.Hide();
                                   }
                               }
                           }));
            }
        }

        private RelayCommand _step2Command;

        /// <summary>
        /// Gets the Step2Command.
        /// </summary>
        public RelayCommand Step2Command
        {
            get
            {
                return _step2Command
                       ?? (_step2Command = new RelayCommand(
                           async() =>
                           {
                               var courier = new Courier
                               {
                                   Name = Name,
                                   Surname = Surname,
                                   PhoneNumber = PhoneNumber,
                                   Email = UserPrincipalName
                               };
                               await Task.Run(() => _dataService.Insert(courier));

                               _navigationService.NavigateTo(
                                   ViewModelLocator.CourierInitialiserTimeIntervalsPageKey, courier);
                           }));
            }
        }
    }
}