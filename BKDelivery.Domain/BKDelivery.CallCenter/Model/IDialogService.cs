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
        void Show(DialogType type, string message);
    }
}
