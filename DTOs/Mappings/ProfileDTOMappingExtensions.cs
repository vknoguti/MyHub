using MyHub.Entities;

namespace MyHub.DTOs.Mappings
{
    public static class ProfileDTOMappingExtensions
    {
        public static Profile<TKey>? ToProfile<TKey>(this RegisterProfileDTO<TKey> registerProfileDTO) where TKey : IEquatable<TKey>
        {
            return new Profile<TKey>
            {
                UserId = registerProfileDTO.UserId,
                BirthDate = registerProfileDTO.BirthDate,
                PhoneNumber = registerProfileDTO?.PhoneNumber,
                FullName = registerProfileDTO?.FullName
            };
        }

        public static RegisterProfileDTO<TKey>? ToRegisterProfileDTO<TKey>(this Profile<TKey> profile)
            where TKey: IEquatable<TKey>
        {
            return new RegisterProfileDTO<TKey>
            {
                UserId = profile.UserId,
                BirthDate = profile.BirthDate,
                FullName = profile.FullName,
                PhoneNumber = profile.PhoneNumber
            };
        }
    }
}
