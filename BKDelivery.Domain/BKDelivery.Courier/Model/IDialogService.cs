using BKDelivery.Courier.Helpers;

namespace BKDelivery.Courier.Model
{
    public interface IDialogService
    {
        void Show(DialogType type, string message);
        void Hide();
    }
}
