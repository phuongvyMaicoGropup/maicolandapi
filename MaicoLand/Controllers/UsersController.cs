using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaicoLand.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<bool> Register([FromBody] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.Register(request);
                if (!result)
                {
                    return false; 
                }
                return true; 
            }
            return true; 
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.Authenticate(request);
                if (string.IsNullOrEmpty(result))
                {
                    return BadRequest("Username or password is uncorrect");
                }
                return Ok(new { token = result });
            }
            return BadRequest(ModelState);
        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var item = await _userRepository.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }
        [HttpGet("checkphoneaccount")]
        public async Task<String> CheckPhoneAccount(string phone)
        {
            string item = await _userRepository.CheckPhone(phone);
            StringBuilder sb = new StringBuilder(item);
            

            if (item.Length <=5)
            {
                for (int i = 1; i < item.Length - 1; i++)
                {
                    sb[i] = '*';
                }
                sb[item.Length-1]= item[item.Length-1];
            }
            else
            {
                for (int i = 1; i < item.Length - 2; i++)
                {
                    sb[i] = '*';
                }
                sb[item.Length - 1] = item[item.Length - 1];
                sb[item.Length - 2] = item[item.Length - 2];

            }

            return sb.ToString();
        }

        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<bool> ChangePassword(string password, string phone)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.ChangePassword(password, phone);
                if (!result)
                {
                    return false; 
                }
                return true; 
            }
            return false; 
        }
        //[AllowAnonymous]
        //public async Task<IActionResult> Logout()
        //{
        //    await _userRepository.Logout();
        //    return Ok(); 

        //}
    }
}
