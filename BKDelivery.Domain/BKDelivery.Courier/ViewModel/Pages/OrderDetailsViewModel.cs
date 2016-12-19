using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OrderDetailsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public OrderDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _goBack;

        public RelayCommand GoBack
        {
            get
            {
                return _goBack
                    ?? (_goBack = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.OrdersPageKey);
                    }));
            }
        }
    }
}