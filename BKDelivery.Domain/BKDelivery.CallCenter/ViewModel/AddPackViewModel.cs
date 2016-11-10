using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace BKDelivery.CallCenter.ViewModel
{
    public class AddPackViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;

        private RelayCommand _addCommand;

        public AddPackViewModel(INavigationService navigationService, IDataService dataService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private Order SelectedOrder => _navigationService.Parameter as Order;

        private double _weight;
        private decimal _cost;
        private Category _selectedcategory;
        private List<Category> _categoriesCollection;

        public List<Category> CategoriesCollection
        {
            get { return new List<Category>(_dataService.CategoriesAll()); }
            set { Set(() => CategoriesCollection, ref _categoriesCollection, value); }
        }

        public Category SelectedCategory
        {
            get { return _selectedcategory; }
            set { Set(() => SelectedCategory, ref _selectedcategory, value); }
        }

        public double Weight
        {
            get { return _weight; }
            set { Set(() => Weight, ref _weight, value); }
        }

        //TODO Koszt musi uwzględniać mnożnik z kategorii i wagę, żeby ceny się jakoś różniły
        public decimal Cost
        {
            get { return _cost; }
            set { Set(() => Cost, ref _cost, value); }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                       ?? (_addCommand = new RelayCommand(
                           () =>
                           {
                               //TODO Koszt musi być wyliczany na podstawie wagi i kategorii nie wpisuje się go
                               if (SelectedCategory == null)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error, "Please select category.");
                               }
                               else
                               {
                                   var pack = new Package
                                   {
                                       Weight = Weight,
                                       Cost = Cost,
                                       CategoryId = SelectedCategory.CategoryId,
                                   };
                                   _dataService.PackageAdd(pack, SelectedOrder);
                                   _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2);
                               }
                           }));
            }
        }
    }
}
