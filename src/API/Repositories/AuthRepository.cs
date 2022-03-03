using API.Data;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System;
using System.Text;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly APIContext _context;

        public AuthRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<LoginSuccess> Login(Login login)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user == null)
                return null;

            //if (user.Payed == false)
            //    return null;

            bool verified = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

            if (!verified)
                return null;

            var refreshToken = API.Services.TokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            user.Password = "";

            return new LoginSuccess
            {
                User = user,
                Token = API.Services.TokenService.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<LoginSuccess> Register(Register register)
        {
            bool userExists = await _context.Users.AnyAsync(x => x.Email == register.Email);

            if(userExists)
                return null;

            var refreshToken = API.Services.TokenService.GenerateRefreshToken();

            var options = new CustomerCreateOptions
            {
                Name = register.UserName,
                Email = register.Email,
                Description = "Apointment Management Costumer",
            };

            User user = new User
            {
                UserName = register.UserName,
                Email = register.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = "Owner",
                RefreshToken = refreshToken
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.Password = "";

            return new LoginSuccess{
                User = user,
                Token = API.Services.TokenService.GenerateToken(user),
                RefreshToken=refreshToken
            };
        }

        public async Task<RefreshTokenSuccess> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            User user = await _context.Users.FindAsync(refreshTokenRequest.UserId);

            string savedRefreshToken = user.RefreshToken;

            if (savedRefreshToken != refreshTokenRequest.RefreshToken)
                throw new SecurityTokenException("Invalid Token");

            var newJwtToken = API.Services.TokenService.GenerateToken(user);

            var newRefreshToken = API.Services.TokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new RefreshTokenSuccess
            {
                Token = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
