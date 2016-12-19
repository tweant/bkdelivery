using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{

    public class TimetableViewModel : ViewModelBase
    {

        public TimetableViewModel()
        {
        }

        private DateTime? _selectedDate;

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set { Set(() => SelectedDate, ref _selectedDate, value); }
        }

        private RelayCommand _filterByDateCommand;

        public RelayCommand FilterByDateCommand
        {
            get
            {
                return _filterByDateCommand
                    ?? (_filterByDateCommand = new RelayCommand(
                    () =>
                    {

                    }));
            }
        }
    }
}