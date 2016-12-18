using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.CallCenter.Helpers;

namespace BKDelivery.CallCenter.Model
{
    public interface IDialogService
    {
        // V 1.1
        NotificationElement Show(DialogType type, string message, int? time = null);
        void Hide(NotificationElement notification);
        void ManualUpdate(NotificationElement notification, double percentage);

        // V 1.0
        //void Show(DialogType type, string message);
        //void Hide();
    }
}
