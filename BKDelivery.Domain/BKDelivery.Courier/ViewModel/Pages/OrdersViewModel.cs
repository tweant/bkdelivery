using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Address = BKDelivery.Domain.Model.Address;

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
        private readonly INavigationService _navigationService;
        private IDialogService _dialogService;
        private Order _selectedOrder;


        public OrdersViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _usersFilteredCollection = new CollectionViewSource();
            OrdersCollection = new ObservableCollection<Order>();
            _usersFilteredCollection.Source = OrdersCollection;
            _usersFilteredCollection.Filter += usersCollection_Filter;
        }

        private List<string> _orderStatusList = new List<string> {"Active", "Inactive", "All"};
        private string _selectedOrderStatus = "All";
        private bool _isSearchClicke = false;
        private RelayCommand _cleanupCommand;

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               var dialog = _dialogService.Show(Helpers.DialogType.BusyWaiting, "Updating orders list");

                               //TODO Download from API
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
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       },
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       },
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       },
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       }
                                   },
                                   Client = new Client {Name = "AT&T", PhoneNumber = 647945125},
                                   TimeInterval =
                                       new TimeInterval
                                       {
                                           Start = DateTime.Now,
                                           End = new DateTime(2014, 12, 15, 15, 12, 45)
                                       },
                                   OrderId = 1
                               };
                               Order futureOrder = new Order
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
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       },
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       },
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       },
                                       new Package
                                       {
                                           Category = new Category {Name = "Furniture"},
                                           Cost = (decimal) 1458.45,
                                           Weight = 45.8
                                       }
                                   },
                                   Client = new Client {Name = "AT&T", PhoneNumber = 647945125},
                                   TimeInterval =
                                       new TimeInterval
                                       {
                                           Start = DateTime.Now,
                                           End = new DateTime(2017, 12, 15, 15, 12, 45)
                                       },
                                   OrderId = 2
                               };
                               var temp = new ObservableCollection<Order>();


                               for (int i = 0; i < 10; i++)
                               {
                                   OrdersCollection.Add(_editedOrder);
                                   OrdersCollection.Add(futureOrder);
                               }
                               OrdersCollection = temp;

                               //DO NOT DELETE
                               
                               
                               _usersFilteredCollection.View.Refresh();
                               dialog.Hide();
                           }));
            }
        }

        //FILTERED COLLECTION
        private ObservableCollection<Order> _ordersCollection;
        private CollectionViewSource _usersFilteredCollection;
        private string _searchQuery = "Search by client's name or order's id";
        private RelayCommand _clearFilters;

        public ObservableCollection<Order> OrdersCollection
        {
            get { return _ordersCollection; }
            set { Set(() => OrdersCollection, ref _ordersCollection, value); }
        }

        public ICollectionView UsersFilteredCollection => _usersFilteredCollection.View;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set { Set(() => SearchQuery, ref _searchQuery, value); }
        }

        private void usersCollection_Filter(object sender, FilterEventArgs e)
        {
            Order order = e.Item as Order;
            if (string.IsNullOrEmpty(SearchQuery) || SearchQuery.Equals("Search by client's name or order's id"))
            {
                e.Accepted = usersCollection_FilterActivity(order);
                return;
            }


            if (order.Client.Name.ToUpper().Contains(SearchQuery.ToUpper()))
            {
                e.Accepted = usersCollection_FilterActivity(order);
                return;
            }
            else
            {
                e.Accepted = false;
            }

            int res;
            if (int.TryParse(SearchQuery, out res) && order.OrderId == res)
            {
                e.Accepted = usersCollection_FilterActivity(order);
                return;
            }
            e.Accepted = false;
        }

        private bool usersCollection_FilterActivity(Order o)
        {
            switch (SelectedOrderStatus)
            {
                case "Active":
                    return o.TimeInterval.End.CompareTo(DateTime.Now) >= 0;
                case "Inactive":
                    return o.TimeInterval.End.CompareTo(DateTime.Now) < 0;
                case "All":
                    return true;
            }
            return false;
        }

        public RelayCommand ClearFiltersCommand
        {
            get
            {
                return _clearFilters
                       ?? (_clearFilters = new RelayCommand(
                           () =>
                           {
                               SearchQuery = "Search by client's name or order's id";
                               SelectedOrderStatus = "All";
                           }));
            }
        }

        //OTHER
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
                Set(() => SelectedOrderStatus, ref _selectedOrderStatus, value);
                _usersFilteredCollection.View.Refresh();
            }
        }

        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set { Set(() => SelectedOrder, ref _selectedOrder, value); }
        }


        //RELAYS
        private RelayCommand _enableSearchCommand;
        private RelayCommand _executeSearchCommand;
        private RelayCommand _editOrder;
        private RelayCommand _showOrderDetails;

        public RelayCommand ExecuteSearchCommand
        {
            get
            {
                return _executeSearchCommand
                       ?? (_executeSearchCommand = new RelayCommand(
                           () =>
                           {
                               IsSearchClicked = false;
                               _usersFilteredCollection.View.Refresh();
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


        public RelayCommand ShowOrderDetails
        {
            get
            {
                return _showOrderDetails
                       ?? (_showOrderDetails = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.OrderDetailsPageKey, SelectedOrder); }));
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
                               _navigationService.NavigateTo(ViewModelLocator.OrderCommentEditPageKey, SelectedOrder);
                           }));
            }
        }
    }
}