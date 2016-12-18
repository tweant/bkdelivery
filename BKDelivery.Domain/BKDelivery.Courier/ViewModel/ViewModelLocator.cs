/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:BKDelivery.Courier.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using BKDelivery.Courier.Model;

namespace BKDelivery.Courier.ViewModel
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
        public static string HomePageKey = "HomePage";
        public static string FacebookLogInPageKey = "FacebookLogInPage";
        public static string StartUpPageKey = "StartUpPage";


        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Configure(HomePageKey, new Uri("/Pages/Home.xaml", UriKind.Relative));
            navigationService.Configure(FacebookLogInPageKey, new Uri("/Pages/FacebookLogIn.xaml", UriKind.Relative));
            navigationService.Configure(StartUpPageKey, new Uri("/Pages/StartUp.xaml", UriKind.Relative));


            SimpleIoc.Default.Register<MainViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            
        }
    }
}