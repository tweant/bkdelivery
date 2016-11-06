using GalaSoft.MvvmLight;
using BKDelivery.CallCenter.Model;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
 
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _addressesButtonCommand;

        /// <summary>
        /// Gets the AddressesButtonCommand.
        /// </summary>
        public RelayCommand AddressesButtonCommand
        {
            get
            {
                return _addressesButtonCommand
                    ?? (_addressesButtonCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddressesPageKey);
                    }));
            }
        }
    }
}