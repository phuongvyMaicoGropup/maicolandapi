using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
{
    [CollectionName("NewsStatus")]
    public class NewsStatus
    {
        //id,status,createday,createdby,updateday,updateby
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { set; get; }
        [BsonElement]
        public List<string> Likes { set; get; }
        [BsonElement]
        public DateTime CreateDate { set; get; }
        [BsonElement]
        public DateTime CreatedBy { set; get; }
        [BsonElement]
        public DateTime UpdateDate { set; get; }
    }
}
