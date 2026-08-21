using MyHub.Entities;

namespace MyHub.DTOs.Mappings
{
    public static class UserDTOMappingExtensions
    {
        public static User<TKey>? ToUser<TKey>(this RegisterUserDTO registerUser, string passwordHash) where TKey : IEquatable<TKey>
        {
            if (registerUser == null) return null;
            var user = new User<TKey>
            {
                UserName = registerUser.UserName,
                PasswordHash = passwordHash,
                Email = registerUser.Email,
            };
            return user;
        }
    
        public static RegisterResponseDTO<TKey>? ToRegisterDTO<TKey>(this User<TKey> user) where TKey: IEquatable<TKey>
        {
            return new RegisterResponseDTO<TKey>
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };
        }

        public static ClaimsUserDTO<TKey> ToClaimsUser<TKey>(this User<TKey> user) where TKey : IEquatable<TKey>
        {
            ClaimsUserDTO<TKey> claims = new ClaimsUserDTO<TKey>
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
