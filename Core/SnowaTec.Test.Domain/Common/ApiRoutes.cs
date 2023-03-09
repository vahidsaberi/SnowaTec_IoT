namespace SnowaTec.Test.Domain.Common
{
    public class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        #region Security
        public static class Accounts
        {
            public const string Register = Base + "/accounts/register";

            public const string UpdateUserInfo = Base + "/accounts/UpdateUserInfo/{userId}";

            public const string Delete = Base + "/accounts/Delete/{userId}/{type}";
            public const string Undelete = Base + "/accounts/Undelete/{userId}/{type}";

            public const string GetByUserId = Base + "/accounts/GetByUserId/{userId}";
            public const string GetByUserName = Base + "/accounts/GetByUserName/{username}";
            public const string GetByPhoneNumber = Base + "/accounts/GetByPhoneNumber/{phoneNumber}";
            public const string GetByAccessToken = Base + "/accounts/GetByAccessToken/{accessToken}";

            public const string CheckExistingUsername = Base + "/accounts/CheckExistingUsername/{username}";

            public const string LoginMobile = Base + "/accounts/LoginMobile/{phone}/{mobileCode}/{createUser:bool?}";
            public const string VerifyMobile = Base + "/accounts/VerifyMobile";

            public const string Login = Base + "/accounts/Login";

            public const string RefreshToken = Base + "/accounts/RefreshToken";
            public const string RevokeToken = Base + "/accounts/RevokeToken";

            public const string ForgotPassword = Base + "/accounts/ForgotPassword";
            public const string ResetPassword = Base + "/accounts/ResetPassword";

            public const string GetUserRoles = Base + "/accounts/GetUserRoles/{userId}";
            public const string AddUserNameToRoleName = Base + "/accounts/AddUserNameToRoleName/{userName}/{roleName}";
            public const string RemoveUserNameFromRoleName = Base + "/accounts/RemoveUserNameFromRoleName/{userName}/{roleName}";
            public const string AddUserIdToRoleId = Base + "/accounts/AddUserIdToRoleId/{userId}/{roleId}";
            public const string RemoveUserIdFromRoleId = Base + "/accounts/RemoveUserIdFromRoleId/{userId}/{roleId}";

            //public const string ConfirmEmail = Base + "/accounts/confirmemail";
            //public const string SavePushTokenAnddeviceInfo = Base + "/accounts/savepushtokenanddeviceinfo";
            //public const string DeletePushTokenAnddeviceInfo = Base + "/accounts/deletepushtokenanddeviceinfo";
        }

        public static class Users
        {
            public const string GetAll = Base + "/users/getall";
            public const string Get = Base + "/users/get/{id}";
            public const string Create = Base + "/users/create";
            public const string Update = Base + "/users/update/{id}";
            public const string Delete = Base + "/users/delete/{id}/{type}";
            public const string UnDelete = Base + "/users/undelete/{id}/{type}";
            public const string Filter = Base + "/users/filter";
            public const string Pagination = Base + "/users/pagination";

            public const string LoginUser = Base + "/users/loginUser";
            public const string LoginMobile = Base + "/users/loginMobile";
            public const string VerifyMobil = Base + "/users/VerifyMobil";
        }

        public static class Roles
        {
            public const string GetAll = Base + "/roles/getall";
            public const string Get = Base + "/roles/get/{id}";
            public const string GetByName = Base + "/roles/getbyname/{name}";
            public const string GetUsersInRole = Base + "/roles/getusersinrole/{rolename}";
            public const string GetAllUsersAndRole = Base + "/roles/getalluserandrole";
            public const string Create = Base + "/roles/create";
            public const string Update = Base + "/roles/update/{id}";
            public const string Filter = Base + "/roles/filter";
        }

        public static class SystemParts
        {
            public const string GetAll = Base + "/systemparts/getall";
            public const string Get = Base + "/systemparts/get/{id}";
            public const string Create = Base + "/systemparts/create";
            public const string Update = Base + "/systemparts/update/{id}";
            public const string Delete = Base + "/systemparts/delete/{id}/{type}";
            public const string UnDelete = Base + "/systemparts/undelete/{id}/{type}";
            public const string Filter = Base + "/systemparts/filter";
        }

        public static class Availabilities
        {
            public const string GetAll = Base + "/availabilities/getall";
            public const string Get = Base + "/availabilities/get/{id}";
            public const string Create = Base + "/availabilities/create";
            public const string Update = Base + "/availabilities/update/{id}";
            public const string Delete = Base + "/availabilities/delete/{id}/{type}";
            public const string UnDelete = Base + "/availabilities/undelete/{id}/{type}";
            public const string Filter = Base + "/availabilities/filter";
            public const string Pagination = Base + "/availabilities/pagination";
        }

        public static class OrganizationCharts
        {
            public const string GetAll = Base + "/organizationcharts/getall";
            public const string Get = Base + "/organizationcharts/get/{id}";
            public const string Create = Base + "/organizationcharts/create";
            public const string Update = Base + "/organizationcharts/update/{id}";
            public const string Delete = Base + "/organizationcharts/delete/{id}/{type}";
            public const string UnDelete = Base + "/organizationcharts/undelete/{id}/{type}";
            public const string Filter = Base + "/organizationcharts/filter";
            public const string Pagination = Base + "/organizationcharts/pagination";
        }

        public static class Permissions
        {
            public const string GetAll = Base + "/permissions/getall";
            public const string Get = Base + "/permissions/get/{id}";
            public const string Create = Base + "/permissions/create";
            public const string Update = Base + "/permissions/update/{id}";
            public const string Delete = Base + "/permissions/delete/{id}/{type}";
            public const string UnDelete = Base + "/permissions/undelete/{id}/{type}";
            public const string Filter = Base + "/permissions/filter";
            public const string Pagination = Base + "/permissions/pagination";
        }

        public static class PermissionItems
        {
            public const string GetAll = Base + "/permissionitems/getall";
            public const string Get = Base + "/permissionitems/get/{id}";
            public const string Create = Base + "/permissionitems/create";
            public const string Update = Base + "/permissionitems/update/{id}";
            public const string Delete = Base + "/permissionitems/delete/{id}/{type}";
            public const string UnDelete = Base + "/permissionitems/undelete/{id}/{type}";
            public const string Filter = Base + "/permissionitems/filter";
            public const string Pagination = Base + "/permissionitems/pagination";
        }
        #endregion

        #region Possibility
        public static class PortalInfos
        {
            public const string GetAll = Base + "/portalinfos/getall";
            public const string Get = Base + "/portalinfos/get/{id}";
            public const string Create = Base + "/portalinfos/create";
            public const string Update = Base + "/portalinfos/update/{id}";
            public const string Delete = Base + "/portalinfos/delete/{id}/{type}";
            public const string UnDelete = Base + "/portalinfos/undelete/{id}/{type}";
        }

        public static class Documents
        {
            public const string Upload = Base + "/documents/upload";

            public const string GetAll = Base + "/documents/getall/{loadThumbnail?}";
            public const string Get = Base + "/documents/get/{id}/{loadThumbnail?}";
            public const string GetFileById = Base + "/documents/GetFileById/{id}";
            public const string GetFileByKey = Base + "/documents/GetFileByKey/{key}";
            public const string Create = Base + "/documents/create";
            public const string Update = Base + "/documents/update/{id}";
            public const string UpdateJustContent = Base + "/documents/UpdateJustContent/{id}";
            //public const string UpdateInfo = Base + "/documents/updateInfo/{id}";
            public const string Delete = Base + "/documents/delete/{id}/{type}";
            public const string UnDelete = Base + "/documents/undelete/{id}/{type}";
            public const string Pagination = Base + "/documents/pagination/{loadThumbnail?}";
            public const string Filter = Base + "/documents/filter/{loadThumbnail?}";
        }

        public static class Fields
        {
            public const string GetAll = Base + "/fields/getall";
            public const string Get = Base + "/fields/get/{id}";
            public const string Create = Base + "/fields/create";
            public const string Update = Base + "/fields/update/{id}";
            public const string Delete = Base + "/fields/delete/{id}/{type}";
            public const string UnDelete = Base + "/fields/undelete/{id}/{type}";
            public const string Pagination = Base + "/fields/pagination";
            public const string Filter = Base + "/fields/filter";
        }

        public static class Tags
        {
            public const string GetAll = Base + "/tags/getall";
            public const string Get = Base + "/tags/get/{id}";
            public const string Create = Base + "/tags/create";
            public const string Update = Base + "/tags/update/{id}";
            public const string Delete = Base + "/tags/delete/{id}/{type}";
            public const string UnDelete = Base + "/tags/undelete/{id}/{type}";
            public const string Pagination = Base + "/tags/pagination";
            public const string Filter = Base + "/tags/filter";
        }
        #endregion

        #region Portal
        public static class Clients
        {
            public const string GetAll = Base + "/clients/getall";
            public const string Get = Base + "/clients/get/{id}";
            public const string Create = Base + "/clients/create";
            public const string Update = Base + "/clients/update/{id}";
            public const string Delete = Base + "/clients/delete/{id}/{type}";
            public const string UnDelete = Base + "/clients/undelete/{id}/{type}";
            public const string Filter = Base + "/clients/filter";
            public const string Pagination = Base + "/clients/pagination";
        }
        #endregion

        #region Recovery
        public static class Backups
        {
            public const string GetAll = Base + "/backups/getall";
            public const string Get = Base + "/backups/get/{id}";
            public const string Create = Base + "/backups/create";
            public const string Update = Base + "/backups/update/{id}";
            public const string Delete = Base + "/backups/delete/{id}/{type}";
            public const string UnDelete = Base + "/backups/undelete/{id}/{type}";
            public const string Filter = Base + "/backups/filter";
            public const string Pagination = Base + "/backups/pagination";
        }

        public static class Recoveries
        {
            public const string TestExistTabel = Base + "/recoveries/TestExistTabel/{schema}/{tableName}";
            public const string TestRestore = Base + "/recoveries/TestRestore/{schema}/{tableName}";
        }
        #endregion

        #region Possibility
        public static class Brokers
        {
            public const string Connect = Base + "/brokers/connect";
            public const string Disconnect = Base + "/brokers/disconnect";
            public const string Subscribe = Base + "/brokers/subscribe";
            public const string Send = Base + "/brokers/send";
        }
        #endregion

        #region Test
        public static class Sections
        {
            public const string GetAll = Base + "/sections/getall";
            public const string Get = Base + "/sections/get/{id}";
            public const string Create = Base + "/sections/create";
            public const string Update = Base + "/sections/update/{id}";
            public const string Delete = Base + "/sections/delete/{id}";
            public const string UnDelete = Base + "/sections/undelete/{id}/{type}";
            public const string Filter = Base + "/sections/filter";
            public const string Pagination = Base + "/sections/pagination";
        }

        public static class Devices
        {
            public const string GetAll = Base + "/devices/getall";
            public const string Get = Base + "/devices/get/{id}";
            public const string Create = Base + "/devices/create";
            public const string Update = Base + "/devices/update/{id}";
            public const string Delete = Base + "/devices/delete/{id}";
            public const string UnDelete = Base + "/devices/undelete/{id}/{type}";
            public const string Filter = Base + "/devices/filter";
            public const string Pagination = Base + "/devices/pagination";
        }
        #endregion
    }
}
