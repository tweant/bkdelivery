using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using BKDelivery.Courier.Helpers;
using BKDelivery.Courier.ViewModel;

namespace BKDelivery.Courier.Model
{
    public class DialogService : IDialogService
    {
        private ObservableCollection<UIElement> _notificationsCollection;

        public NotificationElement Show(DialogType type, string message,int? time=null)
        {
            if (!EnsureDialog()) return null;
            var not = new NotificationElement(type,message, _notificationsCollection,time);
            not.BeginEnterAnimation();
            return not;
        }

        public void Hide(NotificationElement notification)
        {
            notification?.Hide();
        }

        private bool EnsureDialog()
        {
            object locator;
            try
            {
                locator = Application.Current.FindResource("Locator");
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return false;
            }
            var vmLocator = locator as ViewModelLocator;
            if (vmLocator == null) return false;
            _notificationsCollection = vmLocator.Main.NotificationsCollection;
            return true;
        }
    }
}