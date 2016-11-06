/*
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

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IUnitOfWorkService, UnitOfWorkService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddressesViewModel>();
            SimpleIoc.Default.Register<AddAddressViewModel>();

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Configure(AddressesPageKey, new Uri("AddressesPage.xaml", UriKind.Relative));
            navigationService.Configure(AddAddressPageKey, new Uri("AddAddressPage.xaml", UriKind.Relative));
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

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}