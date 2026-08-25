using MyHub.DTOs;
using MyHub.DTOs.Mappings;
using MyHub.Entities;
using MyHub.Repository;

namespace MyHub.Services
{
    public class ProfileManagerService<TKey> where TKey : IEquatable<TKey>
    {
        private readonly IProfileRepository<Profile<TKey>, TKey> _profileRepository;
        private readonly IUserRepository<User<TKey>, TKey> _userRepository;
        public ProfileManagerService(IProfileRepository<Profile<TKey>, TKey> profileRepository, IUserRepository<User<TKey>, TKey> userRepository) 
        {
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }

        public async Task<RegisterProfileDTO<TKey>?> Register(RegisterProfileDTO<TKey> profileRegister) 
        {
            var userStored = await _userRepository.GetByIdAsync(profileRegister.UserId);
            var profileStored = userStored?.Profile;

            if(userStored is null)
            {
                //User do not exist in database
                return null;
            }
            if(profileStored is not null)
            {
               //Profile already exists in database
                return null;
            }


            var profileToCreate = profileRegister.ToProfile<TKey>();
            if(profileToCreate is null)
            {
                //could not map value from dto to profile
                return null;
            }
            profileToCreate.UserId = profileRegister.UserId;
            Profile<TKey>? created = await _profileRepository.CreateAsync(profileToCreate);
            if(created is null)
            {
                //could not create in database
                return null;
            }
            return created.ToRegisterProfileDTO<TKey>();
        }
    }
}
