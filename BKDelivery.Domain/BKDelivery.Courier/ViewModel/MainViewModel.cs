using System.Collections.ObjectModel;
using System.Windows;
using GalaSoft.MvvmLight;
using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            NotificationsCollection = new ObservableCollection<UIElement>();
        }

        private RelayCommand _addNotification;

        /// <summary>
        /// Gets the AddNotification.
        /// </summary>
        public RelayCommand AddNotification
        {
            get
            {
                return _addNotification
                    ?? (_addNotification = new RelayCommand(
                    () =>
                    {
                        _dialogService.Show(Helpers.DialogType.BusyWaiting, "lol");
                    }));
            }
        }

        /// <summary>
        /// The <see cref="NotificationsCollection" /> property's name.
        /// </summary>
        public const string NotificationsCollectionPropertyName = "NotificationsCollection";

        private ObservableCollection<UIElement> _notificationsCollection;

        /// <summary>
        /// Sets and gets the NotificationsCollection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<UIElement> NotificationsCollection
        {
            get
            {
                return _notificationsCollection;
            }

            set
            {
                if (_notificationsCollection == value)
                {
                    return;
                }

                _notificationsCollection = value;
                RaisePropertyChanged(NotificationsCollectionPropertyName);
            }
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}