using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    public class VisitController : ApiController
    {
        private RestAPIEntities db = new RestAPIEntities();

        // GET api/<controller>
        // GET: api/NewUsers/5
        //[ResponseType(typeof(City))]
        [HttpGet]
        [Route("user/{userid}/visits")]
        public IQueryable<CityVisit> GetCitiesVisited(int userid)
        {
            var cities = from rs in db.Visits
                         where rs.UsersID == userid
                         select new CityVisit()
                         {
                             VisitID = rs.ID,
                             City = rs.City.Name,
                             State = rs.City.State.Name,
                             Abbr = rs.City.State.Abbreviation,
                             UsersID = rs.UsersID,
                             Longitude = rs.City.Longitude,
                             Latitude = rs.City.Latitude,
                             VisitDate = rs.VisitDate
                         };

            return cities;
        }

        // GET: api/NewUsers/5
        //[ResponseType(typeof(User))]
        [HttpGet]
        [Route("user/{userid}/visits/states")]
        public IQueryable<StateInfo> GetStatesVisited(int userid)
        {
            var states = from rs in db.Visits
                         where rs.UsersID == userid
                         select new StateInfo()
                         {
                             ID = rs.City.StateID,
                             Name = rs.City.State.Name,
                             Abbr = rs.City.State.Abbreviation
                         };

            var distinctStates = states.Distinct<StateInfo>();
            return distinctStates;
        }

        [ResponseType(typeof(Visit))]
        [HttpPost]
        [Route("user/{userid}/visits")]
        public async Task<IHttpActionResult> PostVisits(int userid, [FromBody]CityState[] cities)
        {
            User user = db.Users.Where(x => x.ID == userid).FirstOrDefault<User>();
            List<Visit> visits = new List<Visit>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach(CityState cityState in cities)
            {
                var city = (from rs in db.Cities
                           where rs.Name == cityState.City
                           && rs.State.Name == cityState.State
                           select rs).FirstOrDefault<City>();
                if(null != city)
                {
                    Visit visit = new Visit();
                    visit.CityID = city.ID;
                    visit.UsersID = userid;
                    visit.VisitDate = DateTime.Now;
                    visits.Add(visit);
                    db.Visits.Add(visit);
                }
            }

            await db.SaveChangesAsync();

            return Ok(visits);
        }

        [ResponseType(typeof(CityVisit))]
        [HttpPost]
        [Route("user/{userid}/visit/{cityid}")]
        public async Task<IHttpActionResult> PostVisit(int userid, int cityid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = db.Users.Where(x => x.ID == userid).FirstOrDefault<User>();
            City city = db.Cities.Where(x => x.ID == cityid).FirstOrDefault<City>();

            Visit visit = new Visit();
            visit.CityID = city.ID;
            visit.UsersID = userid;
            visit.VisitDate = DateTime.Now;
            Visit newVisit = db.Visits.Add(visit);

            await db.SaveChangesAsync();

            CityVisit cityVisit = new CityVisit()
            {
                VisitID = newVisit.ID,
                City = newVisit.City.Name,
                State = newVisit.City.State.Name,
                Abbr = newVisit.City.State.Abbreviation,
                UsersID = newVisit.UsersID,
                Longitude = newVisit.City.Longitude,
                Latitude = newVisit.City.Latitude,
                VisitDate = newVisit.VisitDate
            };

            return Ok(cityVisit);
        }

        // DELETE: api/NewUsers/5
        [ResponseType(typeof(Visit))]
        [Route("user/{userid}/visit/{visitid}")]
        public async Task<IHttpActionResult> DeleteVisit(int userid, int visitid)
        {
            User user = await db.Users.FindAsync(userid);
            Visit visit = await db.Visits.FindAsync(visitid);
            if (null == visit || null == user)
            {
                return NotFound();
            }

            db.Visits.Remove(visit);
            await db.SaveChangesAsync();

            return Ok(visit);
        }

    }
}
