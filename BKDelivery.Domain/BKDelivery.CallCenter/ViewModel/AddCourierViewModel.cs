using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddCourierViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _saveCommand;
        private RelayCommand _cleanupCommand;

        public AddCourierViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private string _name;
        private string _surname;
        private int _phonenumber;

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
                           }));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               if (PhoneNumber < 100000000 || PhoneNumber > 999999999)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Put correct phone number.");
                               }
                               else if(Name.Length == 0)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Empty name.");
                               }
                               else if(Surname.Length == 0)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error,
                                       "Empty surname.");
                               }
                               else
                               {
                                   var courier = new Courier
                                   {
                                       Name = Name,
                                       Surname = Surname,
                                       PhoneNumber = PhoneNumber
                                   };
                                   _dataService.CourierAdd(courier);
                                   _navigationService.NavigateTo(ViewModelLocator.CourierInitialiserTimeIntervalsPageKey, courier);
                               }
                           }));
            }
        }
    }
}
