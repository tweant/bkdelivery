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
        private readonly IUnitOfWorkService _unitOfWorkService;

        private RelayCommand _addCommand;

        public AddPackViewModel(INavigationService navigationService, IUnitOfWorkService unitOfWorkService)
        {
            _navigationService = navigationService;
            _unitOfWorkService = unitOfWorkService;

            _unitOfWorkService.InitializeTransaction();
            var typesRepo = _unitOfWorkService.UnitOfWork.Repository<Category>();
            CategoriesCollection = new List<Category>(typesRepo.GetOverview());
            _unitOfWorkService.SaveChanges();
        }

        private double _weight;
        private decimal _cost;
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
                               _unitOfWorkService.InitializeTransaction();
                               var addpackRepo = _unitOfWorkService.UnitOfWork.Repository<Package>();
                               var pack = new Package
                               {
                                   Weight = Weight,
                                   Cost = Cost,
                                   Category =
                                       _unitOfWorkService.UnitOfWork.Repository<Category>()
                                           .GetDetail(x => x.CategoryId == SelectedCategory.CategoryId)
                               };
                               addpackRepo.Add(pack);
                               _unitOfWorkService.SaveChanges();
                               _navigationService.NavigateTo(ViewModelLocator.AddressesPageKey);
                           }));
            }
        }
    }
}
