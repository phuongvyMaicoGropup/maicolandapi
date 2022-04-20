using System;
using System.Collections.Generic;
using MaicoLand.Models.Enums;
using MaicoLand.Models.StructureType;

namespace MaicoLand.Models.Requests
{
    public class SalePostRequest
    {
        public SalePostRequest()
        {
        }
        public string Title { set; get; }
        public string Content { set; get; }
        public Address Address { set; get; }
        public bool IsAvailable { set; get; }
        public bool IsNegotiable { set; get; }
        public GeoPoint Point { set; get; }
        public double Area { set; get; }
        public double Cost { set; get; }
        public List<string> Images { set; get; } = new List<string>();
        public string CreatedBy { set; get; }
        public int Type { set; get; }
        public bool IsPrivate { set; get; }
    }
}
