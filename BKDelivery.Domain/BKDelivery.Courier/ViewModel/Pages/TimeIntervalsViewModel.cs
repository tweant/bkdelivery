using BKDelivery.Courier.Model;
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
    public class TimeIntervalsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public TimeIntervalsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _addTimeInterval;
        private RelayCommand _editTimeInterval;
        private RelayCommand _deleteTimeInterval;

        
        public RelayCommand DeleteTimeInterval
        {
            get
            {
                return _deleteTimeInterval
                    ?? (_deleteTimeInterval = new RelayCommand(
                    () =>
                    {

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

                    }));
            }
        }

        public RelayCommand AddTimeInterval
        {
            get
            {
                return _addTimeInterval
                    ?? (_addTimeInterval = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.TimeIntervalEditPageKey,null);
                    }));
            }
        }
    }
}