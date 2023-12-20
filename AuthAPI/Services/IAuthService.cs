using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface IAuthService
    {
        Task<(int, string)> CreateUser(SignInModel model, string role);
        Task<(int, string)> Login(LoginModel model);
    }
}