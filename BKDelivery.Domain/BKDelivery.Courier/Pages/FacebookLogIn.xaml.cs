using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BKDelivery.Courier.ViewModel;

namespace BKDelivery.Courier.Pages
{
    /// <summary>
    /// Interaction logic for FacebookLogIn.xaml
    /// </summary>
    public partial class FacebookLogIn : Page
    {
        //#region Private Vars
        //private string p_appID;
        //private string p_scopes;
        //private string p_access_token;
        //private DateTime p_token_expires;
        //private string p_granted_scopes;
        //private string p_denied_scopes;
        //private string p_error;
        //private string p_error_reason;
        //private string p_error_description;
        //#endregion

        //#region Public properties
        //public string access_token { get { return p_access_token; } }
        //public DateTime token_expires { get { return p_token_expires; } }
        //public string granted_scopes { get { return p_granted_scopes; } }
        //public string denied_scopes { get { return p_denied_scopes; } }
        //public string error { get { return p_error; } }
        //public string error_reason { get { return p_error_reason; } }
        //public string error_description { get { return p_error_description; } }
        //#endregion

        private FbLoginViewModel _viewModel = (Application.Current.Resources["Locator"] as ViewModelLocator).FbLogin;

        public FacebookLogIn()
        {
            InitializeComponent();
            //WebBrowser.Navigate(
            //    @"https://www.facebook.com/v2.8/dialog/oauth?client_id={1335179443167660}&response_type=token&redirect_uri={https://www.facebook.com/connect/login_success.html}");
            //string p_appID = "1335179443167660";
            //string p_scopes = "public_profile,email,user_about_me";
            string returnURL = WebUtility.UrlEncode("https://www.facebook.com/connect/login_success.html");
            string scopes = WebUtility.UrlEncode(_viewModel.Scopes);
            _viewModel.StartLoadingPageCommand.Execute(null);
            WebBrowser.Address =
                (string.Format(
                    "https://www.facebook.com/v2.8/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token%2Cgranted_scopes&scope={2}&display=popup",
                    new object[] {_viewModel.AppID, returnURL, scopes}));
            
        }

        private void WebBrowser_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Uri address = null;
                address = new Uri(WebBrowser.Address);
                _viewModel.StopLoadingPageCommand.Execute(null);
                if (address != null)
                    // Check to see if we hit return url
                    if (address.AbsolutePath == "/connect/login_success.html")
                    {
                        // Check for error
                        if (address.Query.Contains("error"))
                        {
                            // Error detected
                            //this.result = System.Windows.Forms.DialogResult.Abort;
                            ExtractURLInfo("?", address.Query);
                            _viewModel.ErrorRetryCommand.Execute(null);
                        }
                        else
                        {
                            //this.result = System.Windows.Forms.DialogResult.OK;
                            ExtractURLInfo("#", address.Fragment);
                            _viewModel.SuccessLoginCommand.Execute(null);
                        }
                    }
            }));
                      
        }

        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void ExtractURLInfo(string inpTrimChar, string urlInfo)
        {
            string fragments = urlInfo.Trim(char.Parse(inpTrimChar)); // Trim the hash or the ? mark
            string[] parameters = fragments.Split(char.Parse("&")); // Split the url fragments / query string 

            // Extract info from url
            foreach (string parameter in parameters)
            {
                string[] name_value = parameter.Split(char.Parse("=")); // Split the input

                switch (name_value[0])
                {
                    case "access_token":
                        _viewModel.AccessToken = name_value[1];
                        break;
                    case "expires_in":
                        double expires = 0;
                        if (double.TryParse(name_value[1], out expires))
                        {
                            _viewModel.TokenExpires = DateTime.Now.AddSeconds(expires);
                        }
                        else
                        {
                            _viewModel.TokenExpires = DateTime.Now;
                        }
                        break;
                    case "granted_scopes":
                        _viewModel.GrantedScopes = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "denied_scopes":
                        _viewModel.DeniedScopes = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error":
                        _viewModel.Error = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error_reason":
                        _viewModel.ErrorReason = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error_description":
                        _viewModel.ErrorDescription = WebUtility.UrlDecode(name_value[1]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}