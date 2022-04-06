using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MaicoLand.Repositories.InterfaceRepositories;
using MongoDB.Driver;
using MaicoLand.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MaicoLand.Models.StructureType;
using MaicoLand.Models.Entities;
using MaicoLand.Models.Requests;

namespace MaicoLand.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private ISendMailService _sendMailService;
        public async Task<bool> CheckEmailAccount(String email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return true; 
            }
            return false; 
        }

        public UserRepository(
            IMaicoLandDatabaseSettings settings, UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, IConfiguration config , ISendMailService sendMailService)
        {
            _sendMailService = sendMailService; 
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
            if (user.EmailConfirmed)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, request.RememberMe);

                if (!result.Succeeded) return null;
                var userInfo = await GetByEmailAsync(user.Email);
                var claims = new[]
                {
                new Claim("id", userInfo.Id),
                new Claim("email", userInfo.Email),
                new Claim("fullName", userInfo.FullName),
                new Claim("userName", userInfo.UserName),
                new Claim("birthDate", userInfo.BirthDate.ToString()),
                new Claim("address", userInfo.Address),
                new Claim("bio", userInfo.Bio),
                new Claim("phoneNumber", userInfo.PhoneNumber),
                new Claim("photoURL", userInfo.PhotoURL),
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null; 
            

        }


        public async Task<bool> Register(RegisterRequest request)
        {
            User user = await _userCollection.Find(x => x.UserName == request.UserName).FirstOrDefaultAsync();
            
            if (user == null)
            {
                AppUser newAppUser = new AppUser
                {
                    UserName = request.UserName,
                    Email = request.Email,
                };

                IdentityResult result = await _userManager.CreateAsync(newAppUser, request.Password);
                AppUser appUser = await _userManager.FindByNameAsync(request.UserName);
                User newUser = new User
                {
                    FullName = request.FullName,
                    UserName = request.UserName,
                    //PhotoURL = "",
                    PhoneNumber = request.PhoneNumber,
                    Email=request.Email,
                };
                await _userCollection.InsertOneAsync(newUser);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = "https://maicoland123.herokuapp.com/two-factor-account?userId=" + appUser.Id+"&code="+code;

                MailContent content = new MailContent
                {
                    To = request.Email,
                    Subject = "Xác thực tài khoản email ",
                    Body = "<p><strong>Xin chào" + request.FullName +" </strong></p> " + "<p> Vui lòng nhấn vào đường <a href=\"" + callbackUrl+"\" > link</a> sau đây để xác thực tài khoản đăng nhập vào MaiCoLand</p>"

                };


                await _sendMailService.SendMail(content);
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

        public async Task<bool> CheckConfirmedEmailAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user.EmailConfirmed)
            {
                return true;
            }
            else return false; 
        }
    }
}
