using System;
using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace BKDelivery.CallCenter.ViewModel
{
    public class CourierInitialiserTimeIntervalsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private ObservableCollection<TimeInterval> _timeIntervalsCollection;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private RelayCommand _saveCourierCommand;
        private RelayCommand _addTimeIntervalCommand;
        private int _itemsCount;
        private RelayCommand _cleanupCommand;

        public CourierInitialiserTimeIntervalsViewModel(INavigationService navigationService, IDataService dataService,
            IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;

            _timeIntervalsCollection = new ObservableCollection<TimeInterval>();
        }
        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               TimeIntervalsCollection = new ObservableCollection<TimeInterval>();
                               ItemsCount = TimeIntervalsCollection.Count;
                           }));
            }
        }

        public Courier SelectedCourier => _navigationService.Parameter as Courier;

        public int ItemsCount
        {
            get { return _itemsCount; }
            set { Set(() => ItemsCount, ref _itemsCount, value); }
        }

        public ObservableCollection<TimeInterval> TimeIntervalsCollection
        {
            get { return _timeIntervalsCollection; }
            set { Set(() => TimeIntervalsCollection, ref _timeIntervalsCollection, value); }
        }

        public DateTime? DateTo
        {
            get { return _dateTo; }
            set { Set(() => DateTo, ref _dateTo, value); }
        }

        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set { Set(() => DateFrom, ref _dateFrom, value); }
        }

        public RelayCommand AddTimeIntervalCommand
        {
            get
            {
                return _addTimeIntervalCommand
                       ?? (_addTimeIntervalCommand = new RelayCommand(
                           () =>
                           {
                               if (DateFrom != null && DateTo != null)
                               {
                                   var interval = new TimeInterval
                                   {
                                       Start = (DateTime) DateFrom,
                                       End = (DateTime) DateTo,
                                       IsTaken = false
                                   };
                                   TimeIntervalsCollection.Add(interval);
                               }
                               ItemsCount = TimeIntervalsCollection.Count;
                           }));
            }
        }

        public RelayCommand SaveCourierCommand
        {
            get
            {
                return _saveCourierCommand
                       ?? (_saveCourierCommand = new RelayCommand(
                           async () =>
                           {
                               await Task.Run(() => _dataService.TimeIntervalAdd(TimeIntervalsCollection, SelectedCourier.CourierId));
                               _navigationService.NavigateTo(ViewModelLocator.HomePageKey);
                               _dialogService.Show(Helpers.DialogType.Success,
                                   "Succesfully created courier " + SelectedCourier.Name + " " + SelectedCourier.Surname +
                                   " with " + ItemsCount + " time intervals.");
                           }));
            }
        }
    }
}