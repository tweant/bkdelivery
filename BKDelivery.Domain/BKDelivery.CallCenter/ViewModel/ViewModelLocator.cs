﻿/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:BKDelivery.CallCenter.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using BKDelivery.CallCenter.Model;

namespace BKDelivery.CallCenter.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public static string AddressesPageKey = "AddressesPage";
        public static string AddAddressPageKey = "AddAddressPage";
        public static string AddCourierPageKey = "AddCourierPage";
        public static string AddClientPageKey = "AddClientPage";
        public static string AddOrderPageKey = "AddOrderPage";
        public static string AddPackPageKey = "AddPackPage";
        public static string ShowClientsPageKey = "ShowClientsPage";
        public static string ShowOrdersPageKey = "ShowOrdersPage";
        public static string ShowOrdersDetailsPageKey = "ShowOrdersDetailsPage";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IUnitOfWorkService, UnitOfWorkService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddressesViewModel>();
            SimpleIoc.Default.Register<AddAddressViewModel>();
            SimpleIoc.Default.Register<AddCourierViewModel>();
            SimpleIoc.Default.Register<AddClientViewModel>();
            SimpleIoc.Default.Register<AddOrderViewModel>();
            SimpleIoc.Default.Register<AddPackViewModel>();
            SimpleIoc.Default.Register<ShowClientsViewModel>();
            SimpleIoc.Default.Register<ShowOrdersViewModel>();
            SimpleIoc.Default.Register<ShowOrdersDetailsViewModel>();


            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Configure(AddressesPageKey, new Uri("AddressesPage.xaml", UriKind.Relative));
            navigationService.Configure(AddAddressPageKey, new Uri("AddAddressPage.xaml", UriKind.Relative));
            navigationService.Configure(AddCourierPageKey, new Uri("AddCourierPage.xaml", UriKind.Relative));
            navigationService.Configure(AddClientPageKey, new Uri("AddClientPage.xaml", UriKind.Relative));
            navigationService.Configure(AddOrderPageKey, new Uri("AddOrderPage.xaml", UriKind.Relative));
            navigationService.Configure(AddPackPageKey, new Uri("AddPackPage.xaml", UriKind.Relative));
            navigationService.Configure(ShowClientsPageKey, new Uri("ShowClientsPage.xaml", UriKind.Relative));
            navigationService.Configure(ShowOrdersPageKey, new Uri("ShowOrdersPage.xaml", UriKind.Relative));
            navigationService.Configure(ShowOrdersDetailsPageKey, new Uri("ShowOrdersDetailsPage.xaml", UriKind.Relative));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddressesViewModel AddressesVm => ServiceLocator.Current.GetInstance<AddressesViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddAddressViewModel AddAddressVm => ServiceLocator.Current.GetInstance<AddAddressViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddCourierViewModel AddCourierVm => ServiceLocator.Current.GetInstance<AddCourierViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddClientViewModel AddClientVm => ServiceLocator.Current.GetInstance<AddClientViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddOrderViewModel AddOrderVm => ServiceLocator.Current.GetInstance<AddOrderViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddPackViewModel AddPackVm => ServiceLocator.Current.GetInstance<AddPackViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ShowClientsViewModel ShowClientsVm => ServiceLocator.Current.GetInstance<ShowClientsViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ShowOrdersViewModel ShowOrdersVm => ServiceLocator.Current.GetInstance<ShowOrdersViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ShowOrdersDetailsViewModel ShowOrdersDetailsVm => ServiceLocator.Current.GetInstance<ShowOrdersDetailsViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}