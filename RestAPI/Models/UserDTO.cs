using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateTimeAdded { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string FullName { get { return string.Format("{0} {1}", Firstname, Lastname); }
        }
    }
}