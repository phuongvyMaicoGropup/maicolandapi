using System;
using System.Collections.Generic;
using MaicoLand.Models.Enums;

namespace MaicoLand.Models.Requests
{
    public class NewsRequest
    {
        public string Title { set; get; }
        public string Content { set; get; }
        public List<string> HashTags { set; get; }= new List<string>();
        public List<string> Images { set; get; }= new List<string>();
        public string CreatedBy { set; get; }
        public NewsType Type { set; get; }
        public bool IsPrivate { set; get; }

    }
}
