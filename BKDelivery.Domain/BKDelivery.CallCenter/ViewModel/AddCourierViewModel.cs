using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddCourierViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWorkService _unitOfWorkService;

        private RelayCommand _saveCommand;

        public AddCourierViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;

        }

        private string _name;
        private string _surname;
        private int _phonenumber;

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

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               _unitOfWorkService.InitializeTransaction();
                               var courierRepo = _unitOfWorkService.UnitOfWork.Repository<Courier>();
                               var courier = new Courier
                               {
                                   Name = Name,
                                   Surname = Surname,
                                   PhoneNumber = PhoneNumber,
                               };
                               courierRepo.Add(courier);
                               _unitOfWorkService.SaveChanges();
                               _navigationService.NavigateTo(ViewModelLocator.AddCourierPageKey);
                           }));
            }
        }
    }
}
