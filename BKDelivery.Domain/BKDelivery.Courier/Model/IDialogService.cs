using BKDelivery.Courier.Helpers;

namespace BKDelivery.Courier.Model
{
    public interface IDialogService
    {
        NotificationElement Show(DialogType type, string message,int? time=null);
        void Hide(NotificationElement notification);
    }
}
