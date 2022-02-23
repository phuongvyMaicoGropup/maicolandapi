using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
{

    [CollectionName("IdentityUser")]
    public class AppUser : MongoIdentityUser<Guid>
    {
    }
}
