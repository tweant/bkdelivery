using System;
using System.Globalization;
using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{
    public class TimeIntervalEditViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public TimeIntervalEditViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        private RelayCommand _cancelCommand;
        private RelayCommand _saveCommand;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private RelayCommand _cleanupCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               if (EndDate == null || StartDate == null)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error, "Invalid dates.");
                                   return;
                               }
                               else
                               {
                                   if (!CheckDate())
                                   {
                                       _dialogService.Show(Helpers.DialogType.Error,
                                           "Single time interval cannot excede 10 hours.");
                                       return;
                                   }
                               }

                               //TODO Add or update to Api depending on whether if new or edit (NavigationService.Parameter)
                               _navigationService.NavigateTo(ViewModelLocator.TimeIntervalsPageKey);
                           }));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                       ?? (_cancelCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.TimeIntervalsPageKey); }));
            }
        }


        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                EstimatedTime = "";
                Set(() => StartDate, ref _startDate, value);
            }
        }


        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                EstimatedTime = "";
                Set(() => EndDate, ref _endDate, value);
            }
        }


        private int _startDateHour = 9;

        public string StartDateHour
        {
            get { return _startDateHour.ToString(); }

            set
            {
                if (_startDateHour.ToString() == value)
                {
                    return;
                }

                int res;
                if (int.TryParse(value, out res) && res >= 0 && res <= 23)
                {
                    _startDateHour = res;

                    EstimatedTime = "";
                    RaisePropertyChanged();
                }
                else
                {
                    _dialogService.Show(Helpers.DialogType.Error, "Enter an hour of type integer between 0-23");
                }
            }
        }

        private int _endDateHour = 15;

        public string EndDateHour
        {
            get { return _endDateHour.ToString(); }

            set
            {
                if (_endDateHour.ToString() == value)
                {
                    return;
                }

                int res;
                if (int.TryParse(value, out res) && res>=0 && res <=23)
                {
                    _endDateHour = res;

                    EstimatedTime = "";
                    RaisePropertyChanged();
                }
                else
                {
                    _dialogService.Show(Helpers.DialogType.Error, "Enter an hour of type integer between 0-23");
                }
            }
        }

        private bool CheckDate()
        {
            if (StartDate != null && EndDate != null)
            {
                var start = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day,
                    int.Parse(StartDateHour),
                    0, 0);

                var end = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day,
                    int.Parse(EndDateHour), 0, 0);
                var res = end.Subtract(start);
                return res.TotalHours <= 10 && res.TotalHours >=0;
            }
            return false;
        }

        private string _estimatedTime;

        public string EstimatedTime
        {
            get
            {
                if (EndDate != null)
                {
                    if (StartDate != null)
                    {
                        var start = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day,
                            int.Parse(StartDateHour),
                            0, 0);
                        var end = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day,
                            int.Parse(EndDateHour), 0, 0);
                        var res = end.Subtract(start);
                        return res.TotalHours.ToString(CultureInfo.CurrentCulture);
                    }
                }
                return "Not possible to establish";
            }

            set
            {
                _estimatedTime = value;
                RaisePropertyChanged();
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
                               if (_navigationService.Parameter != null && _navigationService.Parameter is TimeInterval)
                               {
                                   TimeInterval ti = ((TimeInterval) _navigationService.Parameter);
                                   StartDate = new DateTime(ti.Start.Year, ti.Start.Month, ti.Start.Day);
                                   EndDate = new DateTime(ti.End.Year, ti.End.Month, ti.End.Day);
                                   StartDateHour = ti.Start.Hour.ToString();
                                   EndDateHour = ti.End.Hour.ToString();
                                   EstimatedTime = "";
                               }
                               else
                               {
                                   StartDate = DateTime.Now;
                                   EndDate = DateTime.Now;
                                   EstimatedTime = "";
                               }
                           }));
            }
        }
    }
}