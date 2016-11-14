using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using BKDelivery.Domain.Interfaces;
using System.Threading.Tasks;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddClientViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _saveCommand;

        public AddClientViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private string _name;
        private long _NIP;
        private int _phonenumber;
        private string _email;

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public long NIP
        {
            get { return _NIP; }
            set { Set(() => NIP, ref _NIP, value); }
        }

        public int PhoneNumber
        {
            get { return _phonenumber; }
            set { Set(() => PhoneNumber, ref _phonenumber, value); }
        }
        public string EmailAddress
        {
            get { return _email; }
            set { Set(() => EmailAddress, ref _email, value); }
        }

        private RelayCommand _cleanupCommand;
        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               Name = string.Empty;
                               NIP = 0;
                               PhoneNumber = 0;
                               EmailAddress = string.Empty;
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
                               else if (NIP < 1000000000 || NIP > 9999999999)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Put correct NIP.");
                               }
                               else if (EmailAddress.Length == 0)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Empty email address.");
                               }
                               else
                               {
                                   var client = new Client
                                   {
                                       Name = Name,
                                       NIP = NIP,
                                       PhoneNumber = PhoneNumber,
                                       EmailAddress = EmailAddress,
                                   };
                                   await Task.Run(() => _dataService.Insert(client));
                                   _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                               }
                           }));
            }
        }
    }
}
