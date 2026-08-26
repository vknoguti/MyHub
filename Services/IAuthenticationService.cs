using MyHub.DTOs;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.AuthenticationService;

namespace MyHub.Services
{
    public interface IAuthenticationService
    {
        public Task<BaseResponse1<RegisterResponseDTO>> Register(RegisterUserDTO userRegister);
        public Task<BaseResponse1<LoginResponseDTO>> Login(LoginUserDTO loginUser);

        public Task<BaseResponse1<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO);

        public Task<BaseResponse1<LogOutResponseDTO>> LogOut(Guid userId);
    }
}