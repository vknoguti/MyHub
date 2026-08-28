using MyHub.DTOs.Authentication;
using MyHub.Entities;

namespace MyHub.DTOs.Mappings
{
    public static class UserDTOMappingExtensions
    {
        public static User? ToUser(this RegisterUserDTO registerUser, string passwordHash) 
        {
            if (registerUser == null) return null;
            var user = new User
            {
                UserName = registerUser.UserName,
                PasswordHash = passwordHash,
                Email = registerUser.Email,
            };
            return user;
        }
    
        public static RegisterResponseDTO? ToRegisterResponseDTO(this User user)
        {
            return new RegisterResponseDTO
            {
                Email = user.Email,
                UserName = user.UserName
            };
        }

        public static ClaimsUserDTO ToClaimsUser(this User user) 
        {
            ClaimsUserDTO claims = new ClaimsUserDTO
            {
                IdUser = user.Id,
                UserName = user.UserName
            };
            return claims;
        }

        //public static RegisterUserDTO ToRegisterDTO<TKey>(this User<TKey> user) where TKey: IEquatable<TKey>
        //{
        //    var registerUser = new RegisterUserDTO
        //    {
        //        Email = user.Email,
        //        Name = user.Name,
        //        PasswordHash = user.PasswordHash,
        //        PhoneNumber = user.PhoneNumber,
        //        UserName = user.UserName
        //    };
        //    return registerUser;
        //}
    }
}
