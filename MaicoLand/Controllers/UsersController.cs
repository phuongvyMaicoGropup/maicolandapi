using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.Register(request);
                if (!result)
                {
                    return BadRequest("Register is unsuccessful");
                }
                return Ok(); 
            }
            return BadRequest(ModelState); 
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
        //[AllowAnonymous]
        //public async Task<IActionResult> Logout()
        //{
        //    await _userRepository.Logout();
        //    return Ok(); 
                
        //}
    }
}
