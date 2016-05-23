using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Finance.Controllers
{
    [RoutePrefix("api/Date")]
    public class DateController : ApiController
    {
        private AuthContext _ctx;
        private ApplicationUser _userManager;

        // private string userID;
        private DateController()
        {
            _ctx = new AuthContext();
            UserController userController = new UserController();
            _userManager = userController.GetUser();

        }
        [Authorize(Roles = "Administrator")]
        [Route("")]
        public IHttpActionResult Post(DateModel date)
        {
            try
            {
                var oldDate = _ctx.Dates.Find(date.ID);
                if (oldDate != null) return Put(oldDate.ID, date);
                if (!ModelState.IsValid || date == null)
                {
                    return BadRequest(ModelState);
                }                
                var newDate = _ctx.Dates.Add(date);
                _ctx.SaveChanges();

                if (newDate == null)
                {
                    return BadRequest(ModelState);
                }

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // New Date Method
        public IHttpActionResult Put(int id, DateModel date)
        {
            try
            {
                if (date == null) return BadRequest("Could not read date from body");

                var originalDate = _ctx.Dates.Find(id);

                if (originalDate == null || originalDate.ID != id)
                {
                    return BadRequest("date is not found");
                }
                else
                {
                    date.ID = id;
                }
                _ctx.Entry(originalDate).CurrentValues.SetValues(date);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Update Date Method
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var originalDate = _ctx.Dates.Find(id);

                if (originalDate == null || originalDate.ID != id)
                {
                    return BadRequest("date is not found");
                }
                _ctx.Dates.Remove(originalDate);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Delete Date Method
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {

                List<DateModel> Dates = new List<DateModel>();
                Dates = _ctx.Dates.ToList();
                return Ok(Dates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Date Method
    }
}
