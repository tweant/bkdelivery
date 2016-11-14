using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace BKDelivery.CallCenter.ViewModel
{
    public class ShowOrdersViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _ordersCommand;
        private ObservableCollection<Order> _ordersCollection;

        public ShowOrdersViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private int _orderId;
        private int _clientId;
        private int _courierId;

        public int OrderId
        {
            get { return _orderId; }
            set { Set(() => OrderId, ref _orderId, value); }
        }

        public int ClientId
        {
            get { return _clientId; }
            set { Set(() => ClientId, ref _clientId, value); }
        }
        public int CourierId
        {
            get { return _courierId; }
            set { Set(() => CourierId, ref _courierId, value); }
        }

        public ObservableCollection<Order> OrdersCollection
        {
            get { return _ordersCollection; }
            set { Set(() => OrdersCollection, ref _ordersCollection, value); }
        }

        private Order _selectedOrder;

        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set { Set(() => SelectedOrder, ref _selectedOrder, value); }
        }

        public RelayCommand OrdersButtonCommand
        {
            get
            {
                return _ordersCommand
                       ?? (_ordersCommand = new RelayCommand(
                           () => {
                               {
                                   if (SelectedOrder == null)
                                   {
                                       _dialogService.Show(Helpers.DialogType.Error,"Select order.");
                                   }
                                   else if(SelectedOrder.CourierId == null)
                                   {
                                       _dialogService.Show(Helpers.DialogType.Error,"Wrong order.");
                                   }
                                   else
                                   {
                                       _navigationService.NavigateTo(ViewModelLocator.ShowOrdersDetailsPageKey, SelectedOrder);
                                   }
                               }
                           }));
            }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand
                       ?? (_deleteCommand = new RelayCommand(
                           async () => 
                           {
                               {
                                   if (SelectedOrder == null)
                                   {
                                       _dialogService.Show(Helpers.DialogType.Error, "Select order.");
                                   }
                                   else
                                   {
                                       _dialogService.Show(Helpers.DialogType.BusyWaiting,
                                           "Please wait. Deleteing the order.");
                                       var result3 = await Task.Run(() => _dataService.PackagesByOrder(SelectedOrder.OrderId));
                                       foreach (Package package in result3)
                                       {
                                           await Task.Run(() => _dataService.Delete(package));
                                       }
                                       await Task.Run(() => _dataService.Delete(_dataService.Get<Order>(x => x.OrderId == SelectedOrder.OrderId)));
                                       CleanupCommand.Execute(null);
                                       _dialogService.Hide();
                                       _dialogService.Show(Helpers.DialogType.Success, "Succesfully deleted the order.");
                                   }
                               }
                           }));
            }
        }

        private RelayCommand _cleanupCommand;
        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               _dialogService.Show(Helpers.DialogType.BusyWaiting, "Please wait. Loading orders.");
                               OrdersCollection = new ObservableCollection<Order>();
                               var result = await Task.Run(() => _dataService.SearchOrder(OrderId, ClientId, CourierId));
                               foreach (Order order in result)
                               {
                                   OrdersCollection.Add(order);
                               }
                               OrderId = 0;
                               ClientId = 0;
                               CourierId = 0;
                               _dialogService.Hide();
                           }));
            }
        }
    }
}