    using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MaicoLand.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config; 

        public UserRepository(
            IMaicoLandDatabaseSettings settings, UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _userCollection = database.GetCollection<User>("User");
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user =await _userManager.FindByNameAsync(request.UserName);
            
            if (user == null) return null; 
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe,true);
            if (!result.Succeeded) return null;
            var userInfo = await GetByEmailAsync(user.Email);
            var claims = new[]
            {
                new Claim("id", userInfo.Id),
                new Claim("email", userInfo.Email),
                new Claim("username", userInfo.UserName),
                new Claim("firstName", userInfo.FirstName),
                new Claim("lastName", userInfo.LastName),
                new Claim("phoneNumber", userInfo.PhoneNumber),
                new Claim("photoURL", userInfo.PhotoURL),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<bool> Register(RegisterRequest request)
        {
            AppUser appUser = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, request.Password);
            if (result.Succeeded)
            {
                User newUser = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    PhotoURL = "",
                    PhoneNumber=request.PhoneNumber,

                };
                await _userCollection.InsertOneAsync(newUser);
                return true; 
            }
                return false; 
        }

        public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        public async Task<User> GetAsync(string id)
            => await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<User> GetByEmailAsync(string email)
            => await _userCollection.Find(x => x.Email == email).FirstOrDefaultAsync();
        

        public async Task RemoveAsync(string id)
        => await _userCollection.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateAsync(string id, User updatedUser)
        => await _userCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> CheckPhone(string phone)
        {
            string name = "";
            User user = await _userCollection.Find(x => x.PhoneNumber == phone).FirstOrDefaultAsync();
            Console.WriteLine(user);
            if (user != null)
            {
                name = user.UserName;
            }
            return name;

        }

        public async Task<bool> ChangePassword(string password, string phone)
        {
            try
            {
                String name = (await _userCollection.Find(x => x.PhoneNumber == phone).FirstOrDefaultAsync()).UserName;
                AppUser user = await _userManager.FindByNameAsync(name);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result == IdentityResult.Success)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                return false;
            }
          
        }
    }
}
