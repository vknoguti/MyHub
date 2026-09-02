using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Mappings;
using MyHub.DTOs.ProfileManagerService;
using MyHub.DTOs.ProfileService;
using MyHub.Services.Profile;
using MyHub.Shared;

namespace MyHub.Services
{
    public class ProfileManagerService
    {
        private readonly ApplicationDbContext _dbContext;
        public ProfileManagerService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Result<ProfileResponseDTO>> CreateProfile(CreateProfileDTO profileRegister) 
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == profileRegister.UserId);
            if (!userExists)
            {
                return ProfileErrors.UserNotFound;
            }

            var profileExists = await _dbContext.Profiles.AnyAsync(p => p.UserId == profileRegister.UserId);
            if (profileExists)
            {
                return ProfileErrors.ProfileAlreadyExists;
            }

            var profileToCreate = profileRegister.ToProfile();
            if(profileToCreate is null)
            {
                return ProfileErrors.MapProfileFailed;
            }
            profileToCreate.UserId = profileRegister.UserId;
            await _dbContext.Profiles.AddAsync(profileToCreate);
            var created = await _dbContext.SaveChangesAsync();

            if(created <= 0)
            {
                return ProfileErrors.FailedDatabaseUpdate;
            }

            return profileToCreate.ToProfileResponseDTO();
        }

        public async Task<Result<ProfileResponseDetailedDTO>> GetProfile(ProfileRefDTO profile)
        {
            var profiles = _dbContext.Profiles.Where(t => t.UserId == profile.UserId);
            var resultProfile = await profiles.FirstOrDefaultAsync(t => t.Id == profile.Id);
            if(resultProfile is null)
            {
                return ProfileErrors.ProfileNotFound;
            }

            return resultProfile.ToProfileResponseDetailedDTO();
        }

        public async Task<Result<ProfileResponseDTO>> DeleteProfile(Guid profileId)
        {
            var toDelete = await _dbContext.Profiles.SingleOrDefaultAsync(t => t.Id == profileId);
            if(toDelete is null)
            {
                return ProfileErrors.ProfileNotFound;
            }

            _dbContext.Profiles.Remove(toDelete);
            var isRemoved = await _dbContext.SaveChangesAsync();
            if(isRemoved <= 0)
            {
                return ProfileErrors.FailedDatabaseUpdate;
            }

            return toDelete.ToProfileResponseDTO();
        }
    }
}
