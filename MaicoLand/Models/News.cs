﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
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
        public List<string> HashTags { set; get; }
        [BsonElement]
        public string ImageUrl { set; get; }
        [BsonElement]
        public List<string> Likes { set; get; }
        [BsonElement]
        public DateTime CreateDate { set; get; }
        [BsonElement]
        public string CreatedBy { set; get; }
        [BsonElement]
        public DateTime UpdateDate { set; get; }
    }
}