using MyHub.DTOs.ProfileManagerService;
using MyHub.DTOs.ProfileService;
using MyHub.Entities;
namespace MyHub.DTOs.Mappings
{
    public static class ProfileDTOMappingExtensions
    {
        public static Profile? ToProfile(this MyHub.DTOs.ProfileService.CreateProfileDTO registerProfileDTO) 
        {
            return new Profile
            {
                UserId = registerProfileDTO.UserId,
                BirthDate = registerProfileDTO.BirthDate,
                PhoneNumber = registerProfileDTO?.PhoneNumber,
                FullName = registerProfileDTO?.FullName
            };
        }

        public static ProfileResponseDTO? ToProfileResponseDTO(this Profile profile)
        {
            return new ProfileResponseDTO
            {
                BirthDate = profile.BirthDate,
                PhoneNumber = profile.PhoneNumber,
                FullName = profile.FullName
            };
        }

        public static ProfileResponseDetailedDTO? ToProfileResponseDetailedDTO(this Profile profile)
        {
            return new ProfileResponseDetailedDTO
            {
                BirthDate = profile.BirthDate,
                PhoneNumber = profile.PhoneNumber,
                FullName = profile.FullName,
                Id = profile.Id,
                UserId = profile.UserId
            };
        }
    }
}
