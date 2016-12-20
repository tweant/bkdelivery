using System.Collections.ObjectModel;
using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{
    public class TimeIntervalsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public TimeIntervalsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            TimeIntervalsCollection = new ObservableCollection<TimeInterval>();
        }

        private RelayCommand _addTimeInterval;
        private RelayCommand _editTimeInterval;
        private RelayCommand _deleteTimeInterval;
        private TimeInterval _selectedTimeInterval;
        private ObservableCollection<TimeInterval> _timeIntervalsCollection;


        public RelayCommand DeleteTimeInterval
        {
            get
            {
                return _deleteTimeInterval
                       ?? (_deleteTimeInterval = new RelayCommand(
                           () =>
                           {
                               //TODO api delete call
                               CleanupCommand.Execute(null);
                           }));
            }
        }


        public RelayCommand EditTimeInterval
        {
            get
            {
                return _editTimeInterval
                       ?? (_editTimeInterval = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.TimeIntervalEditPageKey,
                                   SelectedTimeInterval);
                           }));
            }
        }

        public RelayCommand AddTimeInterval
        {
            get
            {
                return _addTimeInterval
                       ?? (_addTimeInterval = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.TimeIntervalEditPageKey, null); }));
            }
        }

        public TimeInterval SelectedTimeInterval
        {
            get { return _selectedTimeInterval; }
            set { Set(() => SelectedTimeInterval, ref _selectedTimeInterval, value); }
        }

        public ObservableCollection<TimeInterval> TimeIntervalsCollection
        {
            get { return _timeIntervalsCollection; }
            set { Set(() => TimeIntervalsCollection, ref _timeIntervalsCollection, value); }
        }

        private RelayCommand _cleanupCommand;

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               var dialog = _dialogService.Show(Helpers.DialogType.BusyWaiting, "Updating time intervals list");

                               //TODO Download from API
                               
                               var temp = new ObservableCollection<TimeInterval>();

                               TimeInterval ti = new TimeInterval
                               {
                                   Start = new System.DateTime(2015, 10, 15, 15, 16, 12),
                                   End = new System.DateTime(2015, 10, 15, 17, 18, 12)
                               };
                               for (int i = 0; i < 10; i++)
                               {
                                   temp.Add(ti);
                                   
                               }
                               TimeIntervalsCollection = temp;

                               //DO NOT DELETE
                               dialog.Hide();
                           }));
            }
        }


    }
}