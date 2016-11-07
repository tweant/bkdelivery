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

        private RelayCommand ButtonCommand;
        private RelayCommand ButtonCommand1;
        private RelayCommand ButtonCommand2;
        private RelayCommand ButtonCommand3;
        private RelayCommand ButtonCommand4;

        /// <summary>
        /// Gets the AddressesButtonCommand.
        /// </summary>
        public RelayCommand CourierButtonCommand
        {
            get
            {
                return ButtonCommand
                    ?? (ButtonCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddCourierPageKey);
                    }));
            }
        }
        public RelayCommand ClientButtonCommand
        {
            get
            {
                return ButtonCommand1
                    ?? (ButtonCommand1 = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddClientPageKey);
                    }));
            }
        }
        public RelayCommand OrderButtonCommand
        {
            get
            {
                return ButtonCommand2
                    ?? (ButtonCommand2 = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey);
                    }));
            }
        }
        public RelayCommand ClientsButtonCommand
        {
            get
            {
                return ButtonCommand3
                    ?? (ButtonCommand3 = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.ShowClientsPageKey);
                    }));
            }
        }
        public RelayCommand OrdersButtonCommand
        {
            get
            {
                return ButtonCommand4
                    ?? (ButtonCommand4 = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.ShowOrdersPageKey);
                    }));
            }
        }

    }
}