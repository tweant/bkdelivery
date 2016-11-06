using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BKDelivery.CallCenter.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddressesViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWorkService _unitOfWorkService;

        private ObservableCollection<Address> _addressTypesCollecion;

        private RelayCommand _addAddressCommand;

        public AddressesViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;

            
        }

        public ObservableCollection<Address> AddressesCollection
        {
            get
            {
                _unitOfWorkService.InitializeTransaction();
                var addressRepo = _unitOfWorkService.UnitOfWork.Repository<Address>();
                _addressTypesCollecion = new ObservableCollection<Address>(addressRepo.GetOverview());
                _unitOfWorkService.SaveChanges();
                return _addressTypesCollecion;
            }
            set
            {
                Set(() => AddressesCollection, ref _addressTypesCollecion, value);
            }
        }

        public RelayCommand AddAddressCommand
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