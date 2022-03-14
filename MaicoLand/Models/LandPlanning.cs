using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
{
    public class LandPlanning
    {
       

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [Required]
        public string Id { set; get; }
        [BsonElement]
        public string Title { set; get; }
        [BsonElement]
        public string CreatedBy { set; get; }
        [BsonElement]
        public string Content { set; get; }
        [BsonElement]
        public string ImageUrl { set; get; }
        [BsonElement]
        public List<string> Likes { set; get; }
        [BsonElement]
        public DateTime CreateDate { set; get; }
        [BsonElement]
        public double LandArea { set; get; }
        [BsonElement]
        public string FilePdfUrl { set; get; }
        [BsonElement]
        public DateTime UpdateDate { set; get; }
        [BsonElement]
        public DateTime ExpirationDate { set; get; }
        [BsonElement]
        public GeoPoint LeftTop { set; get; }
        [BsonElement]
        public GeoPoint RightTop { set; get; }
        [BsonElement]
        public GeoPoint LeftBottom { set; get; }
        [BsonElement]
        public GeoPoint RightBottom { set; get; }

        [BsonElement]
        public Address Address { set; get; }

    }
}
