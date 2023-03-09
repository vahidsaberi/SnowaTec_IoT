namespace OrodGoverment.Web.Contract
{
	public class StaticData
	{
        public const string CONTENT_JSON = "application/json";

        public static class Policy
        {
            public const string SUPERADMIN_ONLY = "SuperAdminOnly";
            public const string ADMIN_ONLY = "AdminOnly";
            public const string MODERATOR_ONLY = "ModeratorOnly";
            public const string BASIC_ONLY = "BasicOnly";
            public const string AUTHENTICATED_ONLY = "AuthenticatedOnly";
        }
    }
}
