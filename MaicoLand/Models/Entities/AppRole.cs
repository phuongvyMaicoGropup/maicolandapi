using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models.Entities
{
    [CollectionName("IdentityRole")]
    public class AppRole :  MongoIdentityRole<Guid>
    {
    }
}
