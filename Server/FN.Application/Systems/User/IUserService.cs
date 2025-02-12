using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Device;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Http;

namespace FN.Application.Systems.User
{
    public interface IUserService
    {
        Task<ApiResult<UserViewModel>> GetById(int id);
        Task<ApiResult<bool>> UpdateAvatar(int userId, IFormFile file);
        Task<ApiResult<bool>> ConfirmEmailChange(UpdateEmailResponse response);
        Task<ApiResult<string>> RequestUpdateMail(int userId, string newEmail);
        Task<ApiResult<string>> RequestForgotPassword(string email);
        Task<ApiResult<bool>> ResetPassword(ForgotPasswordRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
    }
}
