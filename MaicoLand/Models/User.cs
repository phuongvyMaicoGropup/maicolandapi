using MongoDB.Bson.Serialization.Attributes;
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
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { set; get; }
        [BsonElement]
        public string FullName { get; set; }
        [BsonElement]
        public string UserName { set; get; }
        [BsonElement]
        public string Email { get; set; } = "";
        [BsonElement]
        public string PhotoURL { set; get; } = "";
        [BsonElement]
        public string PhoneNumber { set; get; } = "";
        [BsonElement]
        public DateTime BirthDate { set; get; } = DateTime.MinValue; 
        [BsonElement]
        public string Bio { set; get; } = "";
        [BsonElement]
        public String Address { set; get; } = "";
        
 

    }
}
