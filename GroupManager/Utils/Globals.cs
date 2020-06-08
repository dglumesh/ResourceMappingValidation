using System.Configuration;

namespace GroupManager.Utils
{
    public static class Globals
    {
        public const string ConsumerTenantId = "9188040d-6c67-4c5b-b112-36a304b66dad";
        public const string IssuerClaim = "iss";
        public const string Authority = "https://login.microsoftonline.com/common/v2.0/";
        public const string RedirectUri = "https://localhost:44321/";
        public const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
        public const string MicrosoftGraphGroupsApi = "https://graph.microsoft.com/v1.0/groups";
        public const string MicrosoftGraphUsersApi = "https://graph.microsoft.com/v1.0/users";
        public const string MicrosoftAzureManagementApi = "https://management.azure.com/subscriptions/{0}/providers/Microsoft.ManagedServices/registrationDefinitions/{1}?api-version=2019-06-01"; // newly added
        public const string MicrosoftAzureManagementRegAssApi = "https://management.azure.com/subscriptions/{0}/providers/Microsoft.ManagedServices/registrationAssignments/{1}?api-version=2019-06-01"; // newly added
        public const string MicrosoftGraphSubscriptionApi = "https://management.azure.com/subscriptions?api-version=2020-01-01"; //newly added
        public const string AdminConsentFormat = "https://login.microsoftonline.com/{0}/adminconsent?client_id={1}&state={2}&redirect_uri={3}";
        public const string MicrosoftGraphInvite = "https://graph.microsoft.com/v1.0/invitations";
        public const string MicrosfotManagementRoleAssignment = "https://management.azure.com/subscriptions/{0}/providers/Microsoft.Authorization/roleAssignments/{1}?api-version=2020-03-01-preview";
        public const string MicrosoftResourceGraphAPI = "https://management.azure.com/providers/Microsoft.ResourceGraph/resources?api-version=2019-04-01";
        public const string MicrosoftResourceMoveValidate = "https://management.azure.com/subscriptions/{0}/resourceGroups/{1}/validateMoveResources?api-version=2019-10-01";
        public const string BasicSignInScopes = "openid profile email offline_access user.readbasic.all";
        public const string NameClaimType = "name";

        /// <summary>
        /// The Client ID is used by the application to uniquely identify itself to Azure AD.
        /// </summary>
        public static string ClientId { get; } = ConfigurationManager.AppSettings["ida:ClientId"];

        /// <summary>
        /// The ClientSecret is a credential used to authenticate the application to Azure AD.  Azure AD supports password and certificate credentials.
        /// </summary>
        public static string ClientSecret { get; } = ConfigurationManager.AppSettings["ida:ClientSecret"];

        /// <summary>
        /// The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        /// </summary>
        public static string PostLogoutRedirectUri { get; } = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        /// <summary>
        /// The TenantId is the DirectoryId of the Azure AD tenant being used in the sample
        /// </summary>
        public static string TenantId { get; } = ConfigurationManager.AppSettings["ida:TenantId"];
    }
}