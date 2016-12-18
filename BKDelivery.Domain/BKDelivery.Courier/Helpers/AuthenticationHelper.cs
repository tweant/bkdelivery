using System;
using System.Threading.Tasks;
using BKDelivery.Courier.Model;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BKDelivery.Courier.Helpers
{
    internal class AuthenticationHelper
    {
        public static string TokenForUser;
        private static IDialogService _dialogService;

        static AuthenticationHelper()
        {
            _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        }

        public static void ClearToken()
        {
            TokenForUser = null;
        }

        /// <summary>
        /// Get Active Directory Client for User.
        /// </summary>
        /// <returns>ActiveDirectoryClient for User.</returns>
        public static ActiveDirectoryClient GetActiveDirectoryClientAsUser()
        {
            Uri servicePointUri = new Uri(GlobalConstants.ResourceUrl);
            Uri serviceRoot = new Uri(servicePointUri, UserModeConstants.TenantId);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                async () => await AcquireTokenAsyncForUser());
            return activeDirectoryClient;
        }

        /// <summary>
        /// Async task to acquire token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        
        public static async Task<string> AcquireTokenAsyncForUser()
        {
            return await GetTokenForUser();
        }

        /// <summary>
        /// Get Token for User.
        /// </summary>
        /// <returns>Token for user.</returns>
        
        public static async Task<string> GetTokenForUser()
        {
            if (TokenForUser == null)
            {
                var redirectUri = new Uri("https://localhost");
                AuthenticationContext authenticationContext = new AuthenticationContext(UserModeConstants.AuthString,
                    false);
                AuthenticationResult userAuthnResult =
                    await authenticationContext.AcquireTokenAsync(GlobalConstants.ResourceUrl,
                        UserModeConstants.ClientId, redirectUri, new PlatformParameters(PromptBehavior.RefreshSession));
                TokenForUser = userAuthnResult.AccessToken;
                //Console.WriteLine("\n Welcome " + userAuthnResult.UserInfo.GivenName + " " +
                //                  userAuthnResult.UserInfo.FamilyName);
                //Thread.Sleep(1000);
                //_dialogService.Show(DialogType.Success,"Succesfully logged to Azure AD as: " + userAuthnResult.UserInfo.GivenName + " " + userAuthnResult.UserInfo.FamilyName);
            }
            return TokenForUser;
        }
    }
}