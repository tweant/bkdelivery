using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BKDelivery.CallCenter.Helpers
{
    //internal class AppModeConstants
    //{
    //    public const string ClientId = "b7734d97-a93b-40b8-8081-d5add18b23e5"; //MINE PAID
    //    public const string ClientSecret = "Ylaajb1y7bUjyB6WGNaZiR8w1tBNR4C1C7KWEAi+znc=";//MINE PAID
    //    public const string TenantName = "kraskatech.onmicrosoft.com";//MINE PAID
    //    public const string TenantId = "7593d73b-d48a-4f1d-b4b9-38d69f96e848";//MINE PAID
    //    public const string AuthString = GlobalConstants.AuthString + TenantName;
    //}

    internal class UserModeConstants
    {
        public const string TenantId = "7593d73b-d48a-4f1d-b4b9-38d69f96e848";
        public const string ClientId = "01bbd482-3d1e-403e-809e-dcdd5bc32db4"; //MINE PAID
        public const string AuthString = GlobalConstants.AuthString + "common/";
    }

    internal class GlobalConstants
    {
        public const string AuthString = "https://login.microsoftonline.com/";
        public const string ResourceUrl = "https://graph.windows.net";
        public const string GraphServiceObjectId = "00000002-0000-0000-c000-000000000000";
    }

    internal class AzureBkDeliveryConstants
    {
        public const string GroupCouriersObjectId = "9daee55b-90a4-4464-960d-6c456753c829";
    }

    public static class Constants
    {
        public static double Precision = 0.01;

        //Styles
        public static readonly Color BlackColor = Color.FromRgb(36, 36, 36);
        public static readonly Color WhiteColor = Color.FromRgb(255, 255, 255);
        public static readonly SolidColorBrush BlackBrush = new SolidColorBrush(BlackColor);
        public static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(WhiteColor);

        //Times
        public static readonly int NotificationShowTime = 5000;
    }
}
