using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddOrderViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWorkService _unitOfWorkService;

        private RelayCommand _chooseclient;
        private ObservableCollection<Client> _clientsTypesCollecion;

        public AddOrderViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;
        }

        public ObservableCollection<Client> ClientsCollection
        {
            get
            {
                _unitOfWorkService.InitializeTransaction();
                var clientsRepo = _unitOfWorkService.UnitOfWork.Repository<Client>();
                _clientsTypesCollecion = new ObservableCollection<Client>(clientsRepo.GetOverview());
                _unitOfWorkService.SaveChanges();
                return _clientsTypesCollecion;
            }
            set
            {
                Set(() => ClientsCollection, ref _clientsTypesCollecion, value);
            }
        }


        private Client _SelectedClient;
        public Client SelectedClient
        {
            get
            {
                return _SelectedClient;
            }
            set
            {
                if (_SelectedClient != value)
                {
                    _SelectedClient = value;
           
                }
            }
        }


        public RelayCommand ChooseClient
        {
            get
            {
                    return _chooseclient
                       ?? (_chooseclient = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2);
                           }));
             }
        }

    }
}
