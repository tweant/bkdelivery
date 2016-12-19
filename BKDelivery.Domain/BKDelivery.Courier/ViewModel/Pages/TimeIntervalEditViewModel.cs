using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{

    public class TimeIntervalEditViewModel : ViewModelBase
    {

        private INavigationService _navigationService;

        public TimeIntervalEditViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _cancelCommand;
        private RelayCommand _saveCommand;


        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () => { }));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                       ?? (_cancelCommand = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.TimeIntervalsPageKey);
                           }));
            }
        }
    }
}