using GalaSoft.MvvmLight;
using BKDelivery.CallCenter.Model;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _addressesButtonCommand;

        public RelayCommand AddressesButtonCommand
        {
            get
            {
                return _addressesButtonCommand
                       ?? (_addressesButtonCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.AddressesPageKey); }));
            }
        }
    }
}