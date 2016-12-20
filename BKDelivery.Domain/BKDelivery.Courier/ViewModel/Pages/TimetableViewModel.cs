using System;
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
    public class TimetableViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private RelayCommand _filterByDateCommand;
        private DateTime? _selectedDate;
        private RelayCommand _cleanupCommand;

        public TimetableViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _usersFilteredCollection = new CollectionViewSource();
            OrdersCollection = new ObservableCollection<Order>();
            _usersFilteredCollection.Source = OrdersCollection;
            _usersFilteredCollection.Filter += usersCollection_Filter;
            SelectedDate = DateTime.Now;
        }


        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { Set(() => SelectedDate, ref _selectedDate, value); }
        }


        public RelayCommand FilterByDateCommand
        {
            get
            {
                return _filterByDateCommand
                       ?? (_filterByDateCommand = new RelayCommand(
                           () => { _usersFilteredCollection.View.Refresh(); }));
            }
        }


        //FILTERED COLLECTION
        private ObservableCollection<Order> _ordersCollection;
        private CollectionViewSource _usersFilteredCollection;

        public ObservableCollection<Order> OrdersCollection
        {
            get { return _ordersCollection; }
            set { Set(() => OrdersCollection, ref _ordersCollection, value); }
        }

        public ICollectionView UsersFilteredCollection => _usersFilteredCollection.View;

        private void usersCollection_Filter(object sender, FilterEventArgs e)
        {
            if (SelectedDate == null)
            {
                e.Accepted = true;
                return;
            }

            Order order = e.Item as Order;
            if (order != null)
            {
                DateTime date = order.TimeInterval.End;
                if (date.Year == SelectedDate.Value.Year && date.Month == SelectedDate.Value.Month &&
                    date.Day == SelectedDate.Value.Day)
                {
                    e.Accepted = true;
                    return;
                }
            }

            e.Accepted = false;
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               var dialog = _dialogService.Show(Helpers.DialogType.BusyWaiting, "Updating timetable");

                               //TODO Download from API

                               var temp = new ObservableCollection<Order>();


                               for (int i = 0; i < 10; i++)
                               {
                                   Order futureOrder = new Order
                                   {
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
                                       TimeInterval =
                                           new TimeInterval
                                           {
                                               Start = DateTime.Now,
                                               End = DateTime.Now
                                           },
                                   };
                                   OrdersCollection.Add(futureOrder);
                               }


                               //DO NOT DELETE
                               OrdersCollection = temp;
                               _usersFilteredCollection.View.Refresh();
                               dialog.Hide();
                           }));
            }
        }
    }
}