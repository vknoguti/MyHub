using MyHub.DTOs;
using MyHub.Enums;


namespace MyHub.Extensions
{
    public static class AuthenticationResponseExtension
    {
        public static BaseResponse1<TData> GenerateResponse1<TData>(this BaseResponse1<TData> response, AppStatus status, TData? data = default) 
        {
            response.StatusCode = status;
            response.StatusName = status.ToString();
            response.Message = status.GetDescriptionMessage();
            response.Data = data;
            return response;
        }
    }
}
