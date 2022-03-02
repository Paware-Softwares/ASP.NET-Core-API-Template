using API.Models;
using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAuthRepository
    {
        Task<LoginSuccess> Login(Login login);

        Task<LoginSuccess> Register(Register register);

        Task<RefreshTokenSuccess> RefreshToken(RefreshTokenRequest refreshTokenRequest);
    }
}
