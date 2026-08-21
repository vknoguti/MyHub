using System.ComponentModel;

namespace MyHub.Enums
{
    public enum AppStatus
    {
        [Description("Login successful")]
        SuccessLogin,

        [Description("User not found")]
        UserNotFound,

        [Description("Invalid username or password")]
        InvalidCredentials,

        //[Description("Account is locked")]
        //AccountLocked,
        //[Description("Account was suspended")]
        //AccountSuspended,
        //[Description("Email is not verified")]
        //EmailNotVerified,



        [Description("Registration successful")]
        SuccessRegistration,

        [Description("Email is already registered")]
        EmailAlreadyExists,

        [Description("Username is already taken")]
        UsernameAlreadyExists,

        [Description("Password does not meet complexity requirements")]
        WeakPassword,

        [Description("Provided data is invalid")]
        InvalidData,

        [Description("Registration failed due to an unexpected error")]
        Failed,




        [Description("Refresh Token is null")]
        NullRefreshToken,

        [Description("Refresh Token is invalid")]
        InvalidRefreshToken,

        [Description("Renewed Access Token and Refresh Token")]
        SuccessRenewAccessToken,

        [Description("Claims not found in principals")]
        PrincipalsNotFound,

    }
}
