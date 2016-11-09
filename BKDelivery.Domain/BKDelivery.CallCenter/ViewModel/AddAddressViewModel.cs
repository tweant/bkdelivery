using System.Collections.Generic;
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
    public class AddAddressViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;

        private RelayCommand _saveCommand;
        private RelayCommand _backCommand;

        public AddAddressViewModel(INavigationService navigationService, IDataService dataService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            VoivodeshipsCollection = new List<string>
            {
                "dolnośląskie",
                "kujawsko-pomorskie",
                "lubelskie",
                "lubuskie",
                "łódzkie",
                "małopolskie",
                "mazowieckie",
                "opolskie",
                "podkarpackie",
                "podlaskie",
                "pomorskie",
                "śląskie",
                "świętokrzyskie",
                "warmińsko-mazurskie",
                "wielkopolskie",
                "zachodniopomorskie"
            };

            TypesCollection = new List<AddressType>(_dataService.AddressTypesAll());

        }

        private string _street;
        private string _buildingNumber;
        private string _flatNumber;
        private string _postalCode;
        private string _city;
        private string _selectedVoivodeship;
        private AddressType _selectedType;
        private List<AddressType> _typesCollection;
        private List<string> _voivodeshipsCollection;

        private Order SelectedOrder => _navigationService.Parameter as Order;

        public List<string> VoivodeshipsCollection
        {
            get { return _voivodeshipsCollection; }
            set { Set(() => VoivodeshipsCollection, ref _voivodeshipsCollection, value); }
        }

        public List<AddressType> TypesCollection
        {
            get { return _typesCollection; }
            set { Set(() => TypesCollection, ref _typesCollection, value); }
        }

        public AddressType SelectedType
        {
            get { return _selectedType; }
            set { Set(() => SelectedType, ref _selectedType, value); }
        }

        public string SelectedVoivodeship
        {
            get { return _selectedVoivodeship; }
            set { Set(() => SelectedVoivodeship, ref _selectedVoivodeship, value); }
        }

        public string City
        {
            get { return _city; }
            set { Set(() => City, ref _city, value); }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set { Set(() => PostalCode, ref _postalCode, value); }
        }

        public string FlatNumber
        {
            get { return _flatNumber; }
            set { Set(() => FlatNumber, ref _flatNumber, value); }
        }

        public string BuildingNumber
        {
            get { return _buildingNumber; }
            set { Set(() => BuildingNumber, ref _buildingNumber, value); }
        }

        public string Street
        {
            get { return _street; }
            set { Set(() => Street, ref _street, value); }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               var address = new Address
                               {
                                   Street = Street,
                                   BuildingNumber = BuildingNumber,
                                   FlatNumber = FlatNumber,
                                   ZipCode = int.Parse(PostalCode),
                                   City = City,
                                   Country = "Poland",
                                   Voivodeship = SelectedVoivodeship,
                                   AddressTypeId = SelectedType.AddressTypeId,
                                   ClientId=SelectedOrder.ClientId,
                                  
                               };
                               _dataService.AddressAdd(address);
                               _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2);
                           }));
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand
                       ?? (_backCommand = new RelayCommand(
                           () =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2);
                           }));
            }
        }
    }
}