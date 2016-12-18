using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BKDelivery.Courier.Helpers;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

namespace BKDelivery.Courier.Model
{
    internal class AzureAdService
    {
        public static ActiveDirectoryClient _client;
        private static readonly IDialogService _dialogService;
        public static ITenantDetail TenantDetail;

        static AzureAdService()
        {
            _dialogService = SimpleIoc.Default.GetInstance<IDialogService>();
        }

        //TODO Move up 
        public static string ExtractErrorMessage(Exception exception)
        {
            List<string> errorMessages = new List<string>();
            string tabs = "\n";
            while (exception != null)
            {
                tabs += "    ";
                errorMessages.Add(tabs + exception.Message);
                exception = exception.InnerException;
            }
            return string.Join("-\n", errorMessages);
        }

        public static void Logout()
        {
            _client = null;
            TenantDetail = null;
            ClearCookies();
            AuthenticationHelper.ClearToken();
        }

        #region CookiesClear

        private static void ClearCookies()
        {
            const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer,
            int lpdwBufferLength);

        #endregion

        public static bool IsLoggedIn()
        {
            return _client != null && TenantDetail != null;
        }

        [STAThread]
        public static async Task Login()
        {
            try
            {
                _client = AuthenticationHelper.GetActiveDirectoryClientAsUser();
            }
            catch (Exception e)
            {
                _dialogService.Show(DialogType.Error,
                    $"Acquiring a token failed with the following error: {ExtractErrorMessage(e)}");
                //TODO: Implement retry and back-off logic per the guidance given here:http://msdn.microsoft.com/en-us/library/dn168916.aspx
                return;
            }

            try
            {
                TenantDetail = await GetTenantDetails(_client, UserModeConstants.TenantId);
            }
            finally
            {
            }
        }


        private static async Task<ITenantDetail> GetTenantDetails(IActiveDirectoryClient client, string tenantId)
        {
            //*********************************************************************
            // The following section may be run by any user, as long as the app
            // has been granted the minimum of User.Read (and User.ReadWrite to update photo)
            // and User.ReadBasic.All scope permissions. Directory.ReadWrite.All
            // or Directory.AccessAsUser.All will also work, but are much more privileged.
            //*********************************************************************


            //*********************************************************************
            // Get Tenant Details
            // Note: update the string TenantId with your TenantId.
            // This can be retrieved from the login Federation Metadata end point:             
            // https://login.windows.net/GraphDir1.onmicrosoft.com/FederationMetadata/2007-06/FederationMetadata.xml
            //  Replace "GraphDir1.onMicrosoft.com" with any domain owned by your organization
            // The returned value from the first xml node "EntityDescriptor", will have a STS URL
            // containing your TenantId e.g. "https://sts.windows.net/4fd2b2f2-ea27-4fe5-a8f3-7b1a7c975f34/" is returned for GraphDir1.onMicrosoft.com
            //*********************************************************************

            ITenantDetail tenant = null;

            IPagedCollection<ITenantDetail> tenantsCollection = await client.TenantDetails
                .Where(tenantDetail => tenantDetail.ObjectId.Equals(tenantId)).ExecuteAsync();

            List<ITenantDetail> tenantsList = tenantsCollection.CurrentPage.ToList();

            if (tenantsList.Count > 0)
            {
                tenant = tenantsList.First();
            }

            if (tenant == null)
            {
                _dialogService.Show(DialogType.Error, "Tenant not found");
                return null;
            }
            else
            {
                TenantDetail tenantDetail = (TenantDetail) tenant;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Tenant Display Name: " + tenantDetail.DisplayName);

                // Get the Tenant's Verified Domains 
                var initialDomain = tenantDetail.VerifiedDomains.First(x => x.Initial.HasValue && x.Initial.Value);
                sb.AppendLine("Initial Domain Name: " + initialDomain.Name);
                var defaultDomain = tenantDetail.VerifiedDomains.First(x => x.@default.HasValue && x.@default.Value);
                sb.AppendLine("Default Domain Name: " + defaultDomain.Name);

                // Get Tenant's Tech Contacts
                foreach (string techContact in tenantDetail.TechnicalNotificationMails)
                {
                    sb.AppendLine("Tenant Tech Contact: " + techContact);
                }
                sb.Remove(sb.Length - 2, 2);
                _dialogService.Show(DialogType.Success, sb.ToString());
                return tenantDetail;
            }
        }

        public static async Task<bool> IsPrincipalNameAvailable(IActiveDirectoryClient client, string principalName)
        {
            //*********************************************************************
            // People picker
            // Search for a user using text string "Us" match against userPrincipalName, displayName, giveName, surname
            // Requires minimum of User.ReadBasic.All.
            //*********************************************************************

            List<IUser> usersList = null;
            IPagedCollection<IUser> searchResults = null;

            IUserCollection userCollection = client.Users;
            searchResults = await userCollection.Where(user =>
                user.UserPrincipalName.StartsWith(principalName)).Take(1).ExecuteAsync();
            usersList = searchResults.CurrentPage.ToList();


            return usersList.Count == 0;
        }

        public static async Task<User> CreateNewUser(IActiveDirectoryClient client, VerifiedDomain defaultDomain,
            string firstName, string lastName, int phoneNumber, string principalName)
        {
            // **********************************************************
            // Requires Directory.ReadWrite.All or Directory.AccessAsUser.All, and the signed in user
            // must be a privileged user (like a company or user admin)
            // **********************************************************

            User newUser = new User();
            if (defaultDomain.Name != null)
            {
                newUser.DisplayName = firstName + " " + lastName;
                //newUser.UserPrincipalName = firstName.ToLower() + "." + lastName.ToLower() + "@" +
                //                            defaultDomain.Name;
                newUser.UserPrincipalName = principalName;
                newUser.AccountEnabled = true;
                newUser.TelephoneNumber = phoneNumber.ToString();
                newUser.MailNickname = firstName + lastName;
                newUser.PasswordProfile = new PasswordProfile
                {
                    Password = "ChangeMe123!",
                    ForceChangePasswordNextLogin = true
                };
                newUser.UsageLocation = "PL";

                await client.Users.AddUserAsync(newUser);
                _dialogService.Show(DialogType.Success, $"New User {newUser.DisplayName} was created");
            }
            return newUser;
        }

        public static async Task AddUserToGroup(Group group, IUser user)
        {
            // add new user to picked group
            group.Members.Add(user as DirectoryObject);
            await group.UpdateAsync();
            _dialogService.Show(DialogType.Success, $"Assigned user {user.DisplayName} to group {group.DisplayName}");
        }

        public static async Task<Group> FindGroupByObjectId(IActiveDirectoryClient client, string groupId)
        {
            List<IGroup> foundGroups = null;
            IGroup retrievedGroup = null;

            IPagedCollection<IGroup> groupsCollection = await client.Groups
                .Where(g => g.ObjectId.Equals(groupId))
                .ExecuteAsync();
            foundGroups = groupsCollection.CurrentPage.ToList();

            if (foundGroups.Count > 0)
            {
                retrievedGroup = foundGroups[0];
            }
            return (Group) retrievedGroup;
        }
    }
}