using MyHub.Entities;

namespace MyHub.DTOs.Mappings
{
    public static class ProfileDTOMappingExtensions
    {
        public static Profile? ToProfile(this RegisterProfileDTO registerProfileDTO) 
        {
            return new Profile
            {
                UserId = registerProfileDTO.UserId,
                BirthDate = registerProfileDTO.BirthDate,
                PhoneNumber = registerProfileDTO?.PhoneNumber,
                FullName = registerProfileDTO?.FullName
            };
        }

        public static RegisterProfileDTO? ToRegisterProfileDTO(this Profile profile)
        {
            return new RegisterProfileDTO
            {
                UserId = profile.UserId,
                BirthDate = profile.BirthDate,
                FullName = profile.FullName,
                PhoneNumber = profile.PhoneNumber
            };
        }
    }
}
