using System;
using System.Collections.Generic;

namespace MaicoLand.Models
{
    public class NewsRequest
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public List<string> HashTags { set; get; }
        public string ImageUrl { set; get; }
        public string CreateBy { set; get; }
    }
}
