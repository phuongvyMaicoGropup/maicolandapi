using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models.Requests
{
    public class LoginRequest
    {
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool RememberMe { set; get; }
    }
}
