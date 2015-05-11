using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.AD
{
    public enum FilterType
    {
        UserByName,
        UserByLoginAccount,
        OrganizationUnit,
        Group,
        Container,
    }

    public enum Status
    {
        Unknow = 0x0, Active = 0x1, Deleted = 0x2, Disabled = 0x3, NotExisted = 0x4
    }

    internal static class Constants
    {
        public static class AccountProperties
        {
            //http://msdn.microsoft.com/en-us/library/ms677980(v=vs.85).aspx

            public const string FirstName = "givenName";
            public const string LastName = "sn";
            public const string Initials = "initials";
            public const string DisplayName = "displayName";
            public const string Description = "description";
            public const string Office = "physicalDeliveryOfficeName";
            public const string PhoneNumber = "telephoneNumber";
            public const string PhoneOther = "otherTelephone";
            public const string Email = "E-mail-Addresses";
            public const string WebPage = "wWWHomePage";
            public const string WebPageOther = "url";

            public const string UserLogonName = "userPrincipalName";
            public const string UserLogonName_preWin2000 = "sAMAccountname";
            public const string LogonHours = "logonHours";
            public const string LogOnTo = "logonWorkstation";
            public const string LockoutTime = "lockoutTime";
            public const string LockoutDuration = "lockoutDuration";
            public const string UserMustChangePassword = "pwdLastSet";
            public const string AccountExpires = "accountExpires";
            public const string UserAccountControl = "userAccountControl";
        }

        public static class AddressProperties
        {
            public const string Street = "streetAddress";
            public const string POBox = "postOfficeBox";
            public const string City = "l";
            public const string State = "st";
            public const string PostalCode = "postalCode";
            public const string Country_Region = "countryCode";
        }

        public static class MemberOfProperties
        {
            public const string MemberOf = "memberOf";
            public const string SetPrimaryGroup = "primaryGroupID";
        }

        public static class OrganizationProperties
        {
            public const string Department = "department";
            public const string Company = "company";
            public const string ManagerName = "manager";
            public const string DirectReports = "directReports";

        }

        public enum AccountPropertyValues : int
        {
            SCRIPT = 0x0001,
            ACCOUNTDISABLE = 0x0002,
            HOMEDIR_REQUIRED = 0x0008,
            LOCKOUT = 0x0010,
            PASSWD_NOTREQD = 0x0020,
            PASSWD_CANT_CHANGE = 0x0040,
            ENCRYPTED_TEXT_PWD_ALLOWED = 0x0080,
            TEMP_DUPLICATE_ACCOUNT = 0x0100,
            NORMAL_ACCOUNT = 0x0200,
            INTERDOMAIN_TRUST_ACCOUNT = 0x0800,
            WORKSTATION_TRUST_ACCOUNT = 0x1000,
            SERVER_TRUST_ACCOUNT = 0x2000,
            DONT_EXPIRE_PASSWORD = 0x10000,
            MNS_LOGON_ACCOUNT = 0x20000,
            SMARTCARD_REQUIRED = 0x40000,
            TRUSTED_FOR_DELEGATION = 0x80000,
            NOT_DELEGATED = 0x100000,
            USE_DES_KEY_ONLY = 0x200000,
            DONT_REQ_PREAUTH = 0x400000,
            PASSWORD_EXPIRED = 0x800000,
            TRUSTED_TO_AUTH_FOR_DELEGATION = 0x1000000
        }
    }
}
