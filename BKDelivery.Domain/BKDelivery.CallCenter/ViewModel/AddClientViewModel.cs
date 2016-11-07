using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddClientViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWorkService _unitOfWorkService;

        private RelayCommand _saveCommand;

        public AddClientViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;

        }

        private string _name;
        private string _surname;
        private int _phonenumber;
        private string _email;

        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        public string Surname
        {
            get { return _surname; }
            set { Set(() => Surname, ref _surname, value); }
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

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               _unitOfWorkService.InitializeTransaction();
                               var clientRepo = _unitOfWorkService.UnitOfWork.Repository<Client>();
                               var client = new Client
                               {
                                   Name = Name,
                                   Surname = Surname,
                                   PhoneNumber = PhoneNumber,
                                   EmailAddress = EmailAddress,
                               };
                               clientRepo.Add(client);
                               _unitOfWorkService.SaveChanges();
                               _navigationService.NavigateTo(ViewModelLocator.AddClientPageKey);
                           }));
            }
        }
    }
}
