using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    public class StateController : ApiController
    {
        private RestAPIEntities db = new RestAPIEntities();

        [Route("state/{stateid}/cities")]
        public IQueryable<CityInfo> GetCitiesInState(int stateid)
        {
            try
            {
                var cities = from rs in db.Cities
                             where rs.StateID == stateid
                             select new CityInfo()
                             {
                                 ID = rs.ID,
                                 Name = rs.Name,
                                 StateID = rs.StateID,
                                 State = rs.State.Name,
                                 Abbr = rs.State.Abbreviation,
                                 Longitude = rs.Longitude,
                                 Latitude = rs.Latitude
                             };
                return cities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("state")]
        public IQueryable<StateInfo> GetStates()
        {
            try
            {
                var states = from rs in db.States
                             select new StateInfo()
                             {
                                 ID = rs.ID,
                                 Name = rs.Name,
                                 Abbr = rs.Abbreviation
                             };
                return states;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
