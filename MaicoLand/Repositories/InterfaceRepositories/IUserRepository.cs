﻿using MaicoLand.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAsync();

        public Task<User?> GetAsync(string id);
        public Task<bool> Register(RegisterRequest request);
        public Task Logout();

        public Task UpdateAsync(string id, User updatedUser);

        public Task RemoveAsync(string id);
        public Task<string> Authenticate(LoginRequest request);

    }
}
