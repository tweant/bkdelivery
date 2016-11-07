using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddClientViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWorkService _unitOfWorkService;

        private ObservableCollection<Address> _addressTypesCollecion;

        private RelayCommand _addAddressCommand;

        public AddClientViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;

            _unitOfWorkService.InitializeTransaction();
            var addressRepo = _unitOfWorkService.UnitOfWork.Repository<Address>();
            AddressesCollection = new ObservableCollection<Address>(addressRepo.GetOverview());
            _unitOfWorkService.SaveChanges();
        }

        public ObservableCollection<Address> AddressesCollection
        {
            get
            {
                return _addressTypesCollecion;
            }
            set
            {
                Set(() => AddressesCollection, ref _addressTypesCollecion, value);
            }
        }

        public RelayCommand ClientButtonCommand
        {
            get
            {
                return _addAddressCommand
                    ?? (_addAddressCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddAddressPageKey);
                    }));
            }
        }
    }
}
