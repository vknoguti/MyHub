using System.ComponentModel;

namespace MyHub.Shared
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





     




        



        [Description("Password does not meet complexity requirements")]
        WeakPassword,

        [Description("Provided data is invalid")]
        InvalidData,







        [Description("Could not found Credentials")]
        CredentialsNotFound,

        [Description("Claims not found in principals")]
        PrincipalsNotFound,



 

    }
}
