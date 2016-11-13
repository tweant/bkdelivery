using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Interfaces;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;

namespace BKDelivery.CallCenter.ViewModel
{
    public class ShowClientsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;


        private ObservableCollection<Client> _clientsCollection;

        public ShowClientsViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        public ObservableCollection<Client> ClientsCollection
        {
            get { return _clientsCollection; }
            set { Set(() => ClientsCollection, ref _clientsCollection, value); }
        }

        private string _name;
        private long _NIP;
        private int _phonenumber;
        private string _email;

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public long NIP
        {
            get { return _NIP; }
            set { Set(() => NIP, ref _NIP, value); }
        }

        public int PhoneNumber
        {
            get { return _phonenumber; }
            set { Set(() => PhoneNumber, ref _phonenumber, value); }
        }
        public string EmailAddress
        {
            get { return _email; }
            set { Set(() => EmailAddress, ref _email, value); }
        }

        private RelayCommand _cleanupCommand;
        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               _dialogService.Show(Helpers.DialogType.BusyWaiting, "Please wait. Loading clients.");
                               ClientsCollection = new ObservableCollection<Client>();
                               var result = await Task.Run(() => _dataService.SearchClient(Name, NIP, PhoneNumber, EmailAddress));
                               foreach (Client client in result)
                               {
                                   ClientsCollection.Add(client);
                               }
                               Name = null;
                               NIP = 0;
                               PhoneNumber = 0;
                               EmailAddress = null;
                               _dialogService.Hide();
                           }));
            }
        }
    }
}