using System.Collections.ObjectModel;
using BKDelivery.CallCenter.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using BKDelivery.Domain.Interfaces;
using System.Threading.Tasks;

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
        private Category _selectedcategory;
        private List<Category> _categoriesCollection;

        public List<Category> CategoriesCollection
        {
            get { return _categoriesCollection; }
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

        private RelayCommand _cleanupCommand;
        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           async () =>
                           {
                               CategoriesCollection = new List<Category>();
                               var result = await Task.Run(() => _dataService.GetAll<Category>());
                               foreach (Category c in result)
                               {
                                   CategoriesCollection.Add(c);
                               }
                               Weight = 0;
                           }));
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand
                       ?? (_addCommand = new RelayCommand(
                           async () =>
                           {
                               if (SelectedCategory == null)
                               {
                                   _dialogService.Show(Helpers.DialogType.Error, "Please select category.");
                               }
                               else
                               {
                                   var pack = new Package
                                   {
                                       Weight = Weight,
                                       Cost = (decimal)SelectedCategory.Multiplier * (decimal)Weight / 1000,
                                       CategoryId = SelectedCategory.CategoryId,
                                   };
                                   await Task.Run(() => _dataService.Add(pack, SelectedOrder));
                                   _navigationService.NavigateTo(ViewModelLocator.AddOrderPageKey2);
                               }
                           }));
            }
        }
    }
}
