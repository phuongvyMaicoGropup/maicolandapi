using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { set; get; }
        public string Email { get; set; }
        public string PhotoURL { set; get; }
        public string PhoneNumber { set; get; }

    }
}
