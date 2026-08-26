using MyHub.Data;
using MyHub.DTOs;
using MyHub.DTOs.Mappings;

namespace MyHub.Services
{
    public class ProfileManagerService
    {
        private readonly ApplicationDbContext _dbContext;
        public ProfileManagerService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<RegisterProfileDTO?> Register(RegisterProfileDTO profileRegister) 
        {
            var userStored = _dbContext.Users.Find(profileRegister.UserId); 
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

            var profileToCreate = profileRegister.ToProfile();
            if(profileToCreate is null)
            {
                //could not map value from dto to profile
                return null;
            }
            profileToCreate.UserId = profileRegister.UserId;
            await _dbContext.Profiles.AddAsync(profileToCreate);
            var created = await _dbContext.SaveChangesAsync();

            if(created <= 0)
            {
                return null;
            }
            return profileToCreate.ToRegisterProfileDTO();
        }
    }
}
