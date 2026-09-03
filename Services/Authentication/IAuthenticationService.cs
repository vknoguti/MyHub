using MyHub.DTOs;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.AuthenticationService;
using MyHub.Shared;

namespace MyHub.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<Result<RegisterUserResponseDTO>> Register(RegisterUserDTO userRegister);
        public Task<Result<LoginUserResponseDTO>> Login(LoginUserDTO loginUser);

        public Task<Result<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO);

        public Task<Result<LogOutResponseDTO>> LogOut(Guid userId);

        public Task<Result<DeleteUserResponseDTO>> DeleteUser(Guid userId);
    }
}