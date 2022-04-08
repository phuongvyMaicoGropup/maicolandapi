using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MaicoLand.Models;
using MaicoLand.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace MaicoLand.Pages
{
    [AllowAnonymous]
    public class TwoFactorAccountModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<TwoFactorAccountModel> logger;


        

        public TwoFactorAccountModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<TwoFactorAccountModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.logger = logger;
            logger.LogInformation("Confirmed Email service");
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync(string userId, string code, string returnUrl)
        {

            if (userId == null || code == null)
            {
                StatusMessage = "none"; 
            }


            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                StatusMessage = "none";

            }

            code = HttpUtility.UrlEncode(code);
            // Xác thực email
            var result = await _userManager.ConfirmEmailAsync(user, code);
            

            if (result.Succeeded)
            {

                StatusMessage = "Đã xác thực thành công"; 
            }
            else
            {
                StatusMessage = "Lỗi xác nhận email : ";

                var s = (from error in result.Errors
                              select error).ToList();
                s.ForEach(a =>{
                    logger.LogInformation(a.Code + " : " + a.Description);
                    StatusMessage += a.Code + " : " + a.Description;
                }); 
                
            }
        }
    }
}
