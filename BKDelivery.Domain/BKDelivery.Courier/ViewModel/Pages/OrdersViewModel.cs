using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
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
    public class OrdersViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public OrdersViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Order _editedOrder = new Order
            {
                FromAddress =
                new Address
                {
                    Street = "Kasprzaka",
                    BuildingNumber = "31B",
                    FlatNumber = "210",
                    ZipCode = 01234,
                    Country = "Poland",
                    City = "Warsaw",
                    Voivodeship = "Mazowieckie"
                },
                ToAddress =
                new Address
                {
                    Street = "Ordona",
                    BuildingNumber = "7",
                    FlatNumber = "",
                    ZipCode = 01234,
                    Country = "Poland",
                    City = "Warsaw",
                    Voivodeship = "Mazowieckie"
                },
                Packages = new System.Collections.Generic.List<Package>()
            {
                new Package {Category = new Category {Name = "Furniture"}, Cost = (decimal) 1458.45, Weight = 45.8},
                new Package {Category = new Category {Name = "Furniture"}, Cost = (decimal) 1458.45, Weight = 45.8},
                new Package {Category = new Category {Name = "Furniture"}, Cost = (decimal) 1458.45, Weight = 45.8},
                new Package {Category = new Category {Name = "Furniture"}, Cost = (decimal) 1458.45, Weight = 45.8}
            },
                Client = new Client { Name = "AT&T", PhoneNumber = 647945125 },
                TimeInterval = new TimeInterval { Start = DateTime.Now, End = DateTime.Now }
            };

            OrdersCollection = new ObservableCollection<Order>();
            for (int i = 0; i < 10; i++)
            {
                OrdersCollection.Add(_editedOrder);
            }
    }


        private List<string> _orderStatusList = new List<string> {"Active", "Inactive", "All"};
        private string _selectedOrderStatus = "All";
        private bool _isSearchClicke = false;

        public bool IsSearchClicked
        {
            get { return _isSearchClicke; }
            set { Set(() => IsSearchClicked, ref _isSearchClicke, value); }
        }

        public List<string> OrdersStatusList
        {
            get { return _orderStatusList; }
            set { Set(() => OrdersStatusList, ref _orderStatusList, value); }
        }

        public string SelectedOrderStatus
        {
            get { return _selectedOrderStatus; }
            set
            {
                //TODO Filter orders
                Set(() => SelectedOrderStatus, ref _selectedOrderStatus, value);
            }
        }


        //RELAYS
        private RelayCommand _enableSearchCommand;
        private RelayCommand _executeSearchCommand;

        public RelayCommand ExecuteSearchCommand
        {
            get
            {
                return _executeSearchCommand
                       ?? (_executeSearchCommand = new RelayCommand(
                           () =>
                           {
                               IsSearchClicked = false;
                               //TODO Search
                           }));
            }
        }

        public RelayCommand EnableSearchCommand
        {
            get
            {
                return _enableSearchCommand
                       ?? (_enableSearchCommand = new RelayCommand(
                           () => { IsSearchClicked = true; }));
            }
        }

        private RelayCommand _editOrder;
        private RelayCommand _showOrderDetails;

        public RelayCommand ShowOrderDetails
        {
            get
            {
                return _showOrderDetails
                    ?? (_showOrderDetails = new RelayCommand(
                    () =>
                    {
                        //TODO Order detals
                    }));
            }
        }

        public RelayCommand EditOrder
        {
            get
            {
                return _editOrder
                    ?? (_editOrder = new RelayCommand(
                    () =>
                    {
                        //TODO Edit order
                        _navigationService.NavigateTo(ViewModelLocator.OrderCommentEditPageKey);
                    }));
            }
        }


        private ObservableCollection<Order> _ordersCollection;


        public ObservableCollection<Order> OrdersCollection
        {
            get
            {
                return _ordersCollection;
            }
            set
            {
                Set(() => OrdersCollection, ref _ordersCollection, value);
            }
        }
    }
}