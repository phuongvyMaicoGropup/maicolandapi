using MaicoLand.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MaicoLand.Models.Entities
{
    [CollectionName("News")]
    public class News
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [Required]
        public string Id { set; get; }
        [BsonElement]
        [Required]
        public string Title { set; get; }
        [Required]
        [BsonElement]
        public string Content { set; get; }
        [BsonElement]
        public List<string> HashTags { set; get; } = new List<string>(); 
        [BsonElement]
        public List<string> Images { set; get; }=new List<string>();
        [BsonElement]
        public List<string> Likes { set; get; } = new List<string>(); 
        [BsonElement]
        public DateTime CreatedDate { set; get; }
        [BsonElement]
        public string CreatedBy { set; get; }
        [BsonElement]
        public DateTime UpdatedDate { set; get; }
        [BsonElement]
        public NewsType Type { set; get; }
        [BsonElement]
        public int Viewed { set; get; }
        [BsonElement]
        public int Saved { set; get; }
        [BsonElement]
        public bool IsPrivated { set; get; }


    }
}
