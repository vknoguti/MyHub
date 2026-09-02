using MyHub.DTOs;
using MyHub.DTOs.Authentication;
using MyHub.DTOs.AuthenticationService;
using MyHub.Shared;

namespace MyHub.Services
{
    public interface IAuthenticationService
    {
        //public Task<BaseResponse1<RegisterResponseDTO>> Register(RegisterUserDTO userRegister);
        //public Task<BaseResponse1<LoginResponseDTO>> Login(LoginUserDTO loginUser);

        //public Task<BaseResponse1<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO);

        //public Task<BaseResponse1<LogOutResponseDTO>> LogOut(Guid userId);
        public Task<Result<RegisterUserResponseDTO>> Register(RegisterUserDTO userRegister);
        public Task<Result<LoginUserResponseDTO>> Login(LoginUserDTO loginUser);

        public Task<Result<RefreshTokenResponseDTO>> RenewJWTWithRefreshToken(RefreshTokenDTO refreshTokenDTO);

        public Task<Result<LogOutResponseDTO>> LogOut(Guid userId);

        public Task<Result<DeleteUserResponseDTO>> DeleteUser(Guid userId);
    }
}