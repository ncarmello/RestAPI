using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using RestAPI.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using RestAPI.Providers;
using RestAPI.Results;

namespace RestAPI.Controllers
{
    public class UsersController : ApiController
    {
        private RestAPIEntities db = new RestAPIEntities();
        private ApplicationUserManager _userManager;

        public UsersController()
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
        }

        public UsersController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        private int GetUsersIDFromIdentity(IdentityUser idUser)
        {
            if (null == idUser)
            {
                return 0;
            }
            try
            {
                UserMap map = db.UserMaps.Where(x => x.AspNetUsersID == idUser.Id).SingleOrDefault<UserMap>();
                if (null == map)
                    return 0;
                else
                    return map.UsersID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Route("user")]
        public IQueryable<UserInfo> GetUsers()
        {
            try
            {
                var users = from rs in db.Users
                            select new UserInfo()
                            {
                                ID = rs.ID,
                                Firstname = rs.FirstName,
                                Lastname = rs.LastName,
                                DateAdded = rs.DateAdded,
                                DateTimeAdded = rs.DateTimeAdded,
                                LastUpdated = rs.LastUpdated
                            };
                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [ResponseType(typeof(UserInfo))]
        [Route("user/{id}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            try
            {
                User user = await db.Users.FindAsync(id);
                UserInfo info;
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    info = new UserInfo();
                    info.ID = user.ID;
                    info.Firstname = user.FirstName;
                    info.Lastname = user.LastName;
                    info.DateAdded = user.DateAdded;
                    info.DateTimeAdded = user.DateTimeAdded;
                    info.LastUpdated = user.LastUpdated;
                }
                return Ok(info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseType(typeof(UserInfo))]
        [Route("user/login")]
        public async Task<IHttpActionResult> GetLoggedInUser()
        {
            try
            {
                IdentityUser idUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                int userID = GetUsersIDFromIdentity(idUser);
                if (0 == userID)
                {
                    return Unauthorized();
                }
                User user = await db.Users.FindAsync(userID);
                UserInfo info;
                if (user == null)
                {
                    return NotFound();
                }

                info = new UserInfo();
                info.ID = user.ID;
                info.Firstname = user.FirstName;
                info.Lastname = user.LastName;
                info.DateAdded = user.DateAdded;
                info.DateTimeAdded = user.DateTimeAdded;
                info.LastUpdated = user.LastUpdated;

                return Ok(info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (id != user.ID)
                {
                    return Unauthorized();
                }

                db.Entry(user).State = EntityState.Modified;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                db.Users.Add(user);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseType(typeof(User))]
        [HttpDelete]
        [Route("user/{id}")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            try
            {
                User user = await db.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("user/{userid}/visits")]
        public IQueryable<CityVisit> GetCitiesVisited(int userid)
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("user/{userid}/visits/states")]
        public IQueryable<StateInfo> GetStatesVisited(int userid)
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }

        [ResponseType(typeof(UserInfo))]
        [HttpPost]
        [Route("user/{userid}/visits")]
        public async Task<IHttpActionResult> PostVisits(int userid, [FromBody]CityState[] cities)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                IdentityUser idUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                int userID = GetUsersIDFromIdentity(idUser);
                if (userID != userid)
                {
                    return Unauthorized();
                }
                User user = await db.Users.FindAsync(userID);
                if (null == user)
                {
                    return NotFound();
                }

                UserInfo ui = new UserInfo();
                ui.ID = user.ID;
                ui.Firstname = user.FirstName;
                ui.Lastname = user.LastName;

                List<Visit> visits = new List<Visit>();
                foreach (CityState cityState in cities)
                {
                    var city = (from rs in db.Cities
                                where rs.Name == cityState.City
                                && rs.State.Name == cityState.State
                                select rs).FirstOrDefault<City>();
                    if (null != city)
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

                return Ok(ui);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ResponseType(typeof(UserInfo))]
        [Route("user/{userid}/visit/{visitid}")]
        public async Task<IHttpActionResult> DeleteVisit(int userid, int visitid)
        {
            try
            {
                IdentityUser idUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                int userID = GetUsersIDFromIdentity(idUser);
                if (userID != userid)
                {
                    return Unauthorized();
                }
                User user = await db.Users.FindAsync(userID);

                if (null == user)
                {
                    return NotFound();
                }
                UserInfo ui = new UserInfo();
                ui.ID = user.ID;
                ui.Firstname = user.FirstName;
                ui.Lastname = user.LastName;
                Visit visit = await db.Visits.FindAsync(visitid);
                if (null == visit || null == user)
                {
                    return NotFound();
                }

                db.Visits.Remove(visit);
                await db.SaveChangesAsync();

                return Ok(ui);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.ID == id) > 0;
        }
    }
}