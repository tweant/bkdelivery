using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{
    public class OrderDetailsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private RelayCommand _goBack;
        private RelayCommand _cleanupCommand;
        private Order _editedOrder;

        public OrderDetailsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


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

        public Order EditedOrder
        {
            get
            {
                return _editedOrder;
            }
            set
            {
                Set(() => EditedOrder, ref _editedOrder, value);
            }
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               if (_navigationService.Parameter != null && _navigationService.Parameter is Order)
                               {
                                   EditedOrder = (Order)_navigationService.Parameter;
                               }
                               else
                               {
                                   EditedOrder = null;
                               }
                           }));
            }
        }
    }
}