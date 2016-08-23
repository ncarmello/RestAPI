using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class StateInfo
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
    }

    public class CityInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? StateID { get; set; }
        public string State { get; set; }
        public string Abbr { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
    }

    public class CityState
    {
        public string City;
        public string State;
    }
}