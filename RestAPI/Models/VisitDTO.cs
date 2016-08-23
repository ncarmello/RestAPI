using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class CityVisit
    {
        public int VisitID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Abbr { get; set; }
        public int UsersID { get; set; }
        public DateTime VisitDate { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
    }

    public class StateVisit
    {
        public int VisitID { get; set; }
        public string State { get; set; }
        public string Abbr { get; set; }
        public int UsersID { get; set; }
        public DateTime VisitDate { get; set; }
    }
}