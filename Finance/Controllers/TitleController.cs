using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Finance.Controllers
{
    [RoutePrefix("api/Title")]
    public class TitleController : ApiController
    {
        private AuthContext _ctx;
        private ApplicationUser _userManager;

        // private string userID;
        private TitleController()
        {
            _ctx = new AuthContext();
            UserController userController = new UserController();
            _userManager = userController.GetUser();

        }
        [Authorize(Roles = "Administrator")]
        [Route("")]
        public IHttpActionResult Post(TitleModel title)
        {
            try
            {
                var oldTitle = _ctx.Titles.Find(title.ID);
                if (oldTitle != null) return Put(title.ID, title);
                if (!ModelState.IsValid || title == null)
                {
                    return BadRequest(ModelState);
                }
                var newTitle = _ctx.Titles.Add(title);
                _ctx.SaveChanges();

                if (newTitle == null)
                {
                    return BadRequest(ModelState);
                }

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // New Title Method
        [HttpPut]
        public IHttpActionResult Put(int id, TitleModel title)
        {
            try
            {
                if (title == null) return BadRequest("Could not read department from body");

                var originalTitle = _ctx.Titles.Find(id);

                if (originalTitle == null || originalTitle.ID != id)
                {
                    return BadRequest("department is not found");
                }
                else
                {
                    title.ID = id;
                }
                _ctx.Entry(originalTitle).CurrentValues.SetValues(title);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Update Title Method
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var originalTitle = _ctx.Titles.Find(id);

                if (originalTitle == null || originalTitle.ID != id)
                {
                    return BadRequest("department is not found");
                }
                _ctx.Titles.Remove(originalTitle);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Delete Title Method
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {

               // List<TitleModel> titles = new List<TitleModel>();
                var titles = _ctx.Titles.Find(id);
                return Ok(titles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Title Method
        [Route("")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {

                List<TitleModel> titles = new List<TitleModel>();
                titles = _ctx.Titles.ToList();
                return Ok(titles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Title Method
    }
}

