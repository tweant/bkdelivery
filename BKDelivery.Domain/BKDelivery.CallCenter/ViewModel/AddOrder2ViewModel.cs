﻿using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddOrder2ViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWorkService _unitOfWorkService;

        private RelayCommand _saveCommand;
        private RelayCommand _addAddressCommand;
        private RelayCommand _addPackCommand;
        private ObservableCollection<Address> _addressesTypesCollecion;

        public AddOrder2ViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;
        }

        public ObservableCollection<Address> AddressesCollection
        {
            get
            {
                _unitOfWorkService.InitializeTransaction();
                var typesRepo = _unitOfWorkService.UnitOfWork.Repository<Address>();
                _addressesTypesCollecion = new ObservableCollection<Address>(typesRepo.GetOverview());
                _unitOfWorkService.SaveChanges();
                return _addressesTypesCollecion;
            }
            set
            {
                Set(() => AddressesCollection, ref _addressesTypesCollecion, value);
            }
        }

        private Address _SelectedHomeAddress;
        private Address _SelectedDeliveryAddress;
        private Address _SelectedInvokeAddress;
        //private Client _selectedClient;
        private ObservableCollection<Package> _packsTypesCollecion;

        public Address SelectedHomeAddress
        {
            get { return _SelectedHomeAddress; }
            set { Set(() => SelectedHomeAddress, ref _SelectedHomeAddress, value); }
        }

        public Address SelectedDeliveryAddress
        {
            get { return _SelectedDeliveryAddress; }
            set { Set(() => SelectedDeliveryAddress, ref _SelectedDeliveryAddress, value); }
        }

        public Address SelectedInvokeAddress
        {
            get { return _SelectedInvokeAddress; }
            set { Set(() => SelectedInvokeAddress, ref _SelectedInvokeAddress, value); }
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

        public RelayCommand AddPackCommand
        {
            get
            {
                return _addPackCommand
                    ?? (_addPackCommand = new RelayCommand(
                    () =>
                    {
                        _navigationService.NavigateTo(ViewModelLocator.AddPackPageKey);
                    }));
            }
        }

        public ObservableCollection<Package> PacksCollection
        {
            get
            {
                _unitOfWorkService.InitializeTransaction();
                var packsRepo = _unitOfWorkService.UnitOfWork.Repository<Package>();
                _packsTypesCollecion = new ObservableCollection<Package>(packsRepo.GetOverview());
                _unitOfWorkService.SaveChanges();
                return _packsTypesCollecion;
            }
            set
            {
                Set(() => PacksCollection, ref _packsTypesCollecion, value);
            }
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
                                               var orderRepo = _unitOfWorkService.UnitOfWork.Repository<Order>();
                                               var order = new Order
                                               {
                                                   FromAddress = SelectedHomeAddress,
                                                   ToAddress = SelectedDeliveryAddress,
                                                   //InvokeAddress = SelectedInvokeAddress,
                                                   //Client = _selectedClient,
                                                   //Packages = ,
                                               };
                                               orderRepo.Add(order);
                                               _unitOfWorkService.SaveChanges();
                                               _navigationService.NavigateTo(ViewModelLocator.AddressesPageKey);
                                           }));
            }
        }
    }
}
