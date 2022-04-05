using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MaicoLand.Models.Enums;
using MaicoLand.Models.StructureType;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace MaicoLand.Models
{
    
    [CollectionName("SalePost")]
    public class SalePost
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [Required]
        public string Id { set; get; }
        [BsonElement]
        [Required]
        public string Title { set; get; }
        [BsonElement]
        public Address Address { set; get; }
        [BsonElement]
        public GeoPoint Point { set; get; }
        [BsonElement]
        public bool IsAvailable { set; get; }
        [BsonElement]
        public bool IsNegotiable { set; get; }
        
[BsonElement]
        public string Content { set; get; }
        [BsonElement]
        public double Area { set; get; }
        [BsonElement]
        public double Cost { set; get; }
        [BsonElement]
        public List<string> Images { set; get; } = new List<string>();
        [BsonElement]
        public DateTime CreatedDate { set; get; } 
        [BsonElement]
        public string CreatedBy { set; get; }
        [BsonElement]
        public DateTime UpdatedDate { set; get; }
        [BsonElement]
        public SalePostType Type { set; get; }
        [BsonElement]
        public int Views { set; get; }
        [BsonElement]
        public int Saved { set; get; }
        [BsonElement]
        public bool IsPrivate { set; get; }


    }
}
