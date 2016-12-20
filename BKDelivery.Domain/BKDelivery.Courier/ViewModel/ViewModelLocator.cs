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
using BKDelivery.Courier.ViewModel.Pages;

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
        //Page keys
        public static string StartUpPageKey = "StartUpPage";
        public static string OrderCommentEditPageKey = "OrderCommentEditPage";
        public static string OrderDetailsPageKey = "OrderDetailsPage";
        public static string OrdersPageKey = "OrdersPage";
        public static string RoutePageKey = "RoutePage";
        public static string TimeIntervalEditPageKey = "TimeIntervalEditPage";
        public static string TimeIntervalsPageKey = "TimeIntervalsPage";
        public static string TimetablePageKey = "TimetablePage";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Services
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            //Pages
            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Configure(StartUpPageKey, new Uri("/Pages/StartUp.xaml", UriKind.Relative));
            navigationService.Configure(OrderCommentEditPageKey, new Uri("/Pages/OrderCommentEdit.xaml", UriKind.Relative));
            navigationService.Configure(OrderDetailsPageKey, new Uri("/Pages/OrderDetails.xaml", UriKind.Relative));
            navigationService.Configure(OrdersPageKey, new Uri("/Pages/Orders.xaml", UriKind.Relative));
            navigationService.Configure(RoutePageKey, new Uri("/Pages/Route.xaml", UriKind.Relative));
            navigationService.Configure(TimeIntervalEditPageKey, new Uri("/Pages/TimeIntervalEdit.xaml", UriKind.Relative));
            navigationService.Configure(TimeIntervalsPageKey, new Uri("/Pages/TimeIntervals.xaml", UriKind.Relative));
            navigationService.Configure(TimetablePageKey, new Uri("/Pages/Timetable.xaml", UriKind.Relative));

            //ViewModels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<OrderCommentEditViewModel>();
            SimpleIoc.Default.Register<OrderDetailsViewModel>();
            SimpleIoc.Default.Register<OrdersViewModel>();
            SimpleIoc.Default.Register<RouteViewModel>();
            SimpleIoc.Default.Register<TimeIntervalEditViewModel>();
            SimpleIoc.Default.Register<TimeIntervalsViewModel>();
            SimpleIoc.Default.Register<TimetableViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public OrderCommentEditViewModel OrderCommentEdit => ServiceLocator.Current.GetInstance<OrderCommentEditViewModel>();
        public OrderDetailsViewModel OrderDetails => ServiceLocator.Current.GetInstance<OrderDetailsViewModel>();
        public OrdersViewModel Orders => ServiceLocator.Current.GetInstance<OrdersViewModel>();
        public RouteViewModel Route => ServiceLocator.Current.GetInstance<RouteViewModel>();
        public TimeIntervalEditViewModel TimeIntervalEdit => ServiceLocator.Current.GetInstance<TimeIntervalEditViewModel>();
        public TimeIntervalsViewModel TimeIntervals => ServiceLocator.Current.GetInstance<TimeIntervalsViewModel>();
        public TimetableViewModel Timetable => ServiceLocator.Current.GetInstance<TimetableViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            
        }
    }
}