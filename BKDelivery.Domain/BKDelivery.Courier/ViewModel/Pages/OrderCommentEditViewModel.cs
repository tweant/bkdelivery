using System;
using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace BKDelivery.Courier.ViewModel.Pages
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class OrderCommentEditViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private RelayCommand _cleanupCommand;

        public OrderCommentEditViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private RelayCommand _cancelCommand;
        private RelayCommand _saveCommand;
        private Order _editedOrder;
        private string _comment;

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand
                       ?? (_saveCommand = new RelayCommand(
                           () =>
                           {
                               //TODO Save comment to database
                               _navigationService.NavigateTo(ViewModelLocator.OrdersPageKey);
                           }));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                       ?? (_cancelCommand = new RelayCommand(
                           () => { _navigationService.NavigateTo(ViewModelLocator.OrdersPageKey); }));
            }
        }

        public Order EditedOrder
        {
            get { return _editedOrder; }
            set { Set(() => EditedOrder, ref _editedOrder, value); }
        }


        public string Comment
        {
            get { return _comment; }
            set { Set(() => Comment, ref _comment, value); }
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               if (_navigationService.Parameter != null && _navigationService.Parameter is Order)
                               {
                                   EditedOrder = (Order)_navigationService.Parameter;
                                   //TODO Comment = EditedOrder.Comment;
                               }
                               else
                               {
                                   EditedOrder = null;
                                   Comment = String.Empty;
                               }
                           }));
            }
        }
    }
}