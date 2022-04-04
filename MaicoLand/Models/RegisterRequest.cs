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
        public string FullName { set; get; }
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
        [Required]
        public string Email { set; get; }

    }
}
