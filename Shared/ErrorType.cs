using System.ComponentModel;

namespace MyHub.Enums
{
    public enum ErrorType
    {
        [Description("Username is already taken")]
        UsernameAlreadyExists,

        [Description("Email is already registered")]
        EmailAlreadyExists,

        [Description("User registration failed due to an unexpected error")]
        UserRegistrationFailed,

        [Description("User not found")]
        UserNotFound,

        [Description("Password did not match")]
        InvalidPassword,

        [Description("Refresh Token is null")]
        NullRefreshToken,

        [Description("Refresh Token is invalid")]
        InvalidRefreshToken,

        [Description("User already logged out")]
        UserAlreadyLoggedOut,

        [Description("Failed to update database")]
        FailedDatabaseUpdate,

        [Description("Profile already exists")]
        ProfileAlreadyExists,

        [Description("Mapping to Profile failed")]
        MapProfileFailed,

        [Description("Profile Not Found")]
        ProfileNotFound,




        [Description("Invalid username or password")]
        InvalidCredentials,



        [Description("Login successful")]
        SuccessLogin,

     


        //[Description("Account is locked")]
        //AccountLocked,
        //[Description("Account was suspended")]
        //AccountSuspended,
        //[Description("Email is not verified")]
        //EmailNotVerified,



        [Description("Registration successful")]
        SuccessRegistration,

        



        [Description("Password does not meet complexity requirements")]
        WeakPassword,

        [Description("Provided data is invalid")]
        InvalidData,





        [Description("Renewed Access Token and Refresh Token")]
        SuccessRenewAccessToken,

        [Description("Could not found Credentials")]
        CredentialsNotFound,

        [Description("Claims not found in principals")]
        PrincipalsNotFound,



 

 

        [Description("LogOut successful")]
        SuccessLogOut
    }
}
