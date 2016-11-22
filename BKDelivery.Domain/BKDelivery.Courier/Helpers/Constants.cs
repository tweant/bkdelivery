using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BKDelivery.Courier.Helpers
{
    public static class Constants
    {
        //Styles
        public static readonly Color BlackColor = Color.FromRgb(36, 36, 36);
        public static readonly Color WhiteColor = Color.FromRgb(255, 255, 255);
        public static readonly SolidColorBrush BlackBrush = new SolidColorBrush(BlackColor);
        public static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(WhiteColor);

        //Times
        public static readonly int NotificationShowTime = 5000;
    }
}
