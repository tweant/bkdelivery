using System;
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
    public class OrderCommentEditViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public OrderCommentEditViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _cancelCommand;
        private RelayCommand _saveCommand;

        private Order _editedOrder = new Order
        {
            FromAddress =
                new Address
                {
                    Street = "Kasprzaka",
                    BuildingNumber = "31B",
                    FlatNumber = "210",
                    ZipCode = 01234,
                    Country = "Poland",
                    City="Warsaw",
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
            Client = new Client {Name = "AT&T", PhoneNumber = 647945125},
            TimeInterval = new TimeInterval {Start = DateTime.Now, End = DateTime.Now}
        };

        private string _comment;

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
                           () => { _navigationService.NavigateTo(ViewModelLocator.OrdersPageKey); }));
            }
        }

        public Order EditedOrder
        {
            get { return _editedOrder; }
            set { Set(() => EditedOrder, ref _editedOrder, value); }
        }


        public string Comment
        {
            get { return _comment; }
            set { Set(() => Comment, ref _comment, value); }
        }
    }
}