
using System;
using System.Collections.Generic;
using MaicoLand.Models.StructureType;

namespace MaicoLand.Models.Responses
{
    public class LandPlanningResponse
    {
        public string Id { set;get; }
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
        public int Views { set; get; }
        public int Saved { set; get; }
        public bool IsPrivate { set; get; }
    }
}
