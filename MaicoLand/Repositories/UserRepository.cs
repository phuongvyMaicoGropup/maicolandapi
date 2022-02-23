﻿using MaicoLand.Models;
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
            IOptions<MaicoLandDatabaseSettings> maicoLandDatabaseSettings, UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config; 
            var mongoClient = new MongoClient("mongodb+srv://phuongvy:123Phuongvy@cluster0.90hui.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            //maicoLandDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase("MaicoLand");
            //maicoLandDatabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>("User");
            //maicoLandDatabaseSettings.Value.NewsCollectionName);
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user =await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null; 
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe,true);
            if (!result.Succeeded) return null;
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
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

        public async Task RemoveAsync(string id)
        => await _userCollection.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateAsync(string id, User updatedUser)
        => await _userCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
