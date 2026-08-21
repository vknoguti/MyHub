using MyHub.DTOs;

namespace MyHub.Services
{
    public interface IAuthenticationService
    {
        public Task<BaseResponse1<RegisterResponseDTO<TKey>>> Register<TKey>(RegisterUserDTO userRegister);
        public Task<BaseResponse1<LoginResponseDTO>> Login(LoginUserDTO loginUser);

        public Task<BaseResponse1<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO);
    }
}