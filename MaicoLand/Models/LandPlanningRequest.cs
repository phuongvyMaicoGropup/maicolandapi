using System;
using System.Collections.Generic;

namespace MaicoLand.Models
{
    public class LandPlanningRequest
    {
        public string Title { set; get; }
        public string CreatedBy { set; get; }
        public string Content { set; get; }
        public string ImageUrl { set; get; }
        public double Area { set; get; }
        public string DetailInfo { set; get; }
        public String ExpirationDate { set; get; }
        public GeoPoint LeftTop { set; get; }
        public GeoPoint RightTop { set; get; }
        public GeoPoint LeftBottom { set; get; }
        public GeoPoint RightBottom { set; get; }
        public Address Address { set; get; }
        public bool IsPrivate { set; get; }

    }
}
