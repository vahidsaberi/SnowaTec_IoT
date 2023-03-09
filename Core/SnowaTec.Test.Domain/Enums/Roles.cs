using System;

namespace SnowaTec.Test.Domain.Enum
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        Moderator,
        Basic
    }

    public static class Constants
    {
        public static readonly int SuperAdmin = 1;
        public static readonly int Admin = 2;
        public static readonly int Moderator = 3;
        public static readonly int Basic = 4;

        public static readonly int SuperAdminUser = 1;
        public static readonly int BasicUser = 2;
    }


}
