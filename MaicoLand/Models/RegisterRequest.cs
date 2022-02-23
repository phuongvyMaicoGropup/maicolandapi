using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
        [Required]
        public string UserName { set; get; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }
        [Required]
        public bool RememberMe { set; get; }
    }
}
