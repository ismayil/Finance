using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Finance.Controllers
{
    [RoutePrefix("api/Department")]
    public class DepartmentController : ApiController
    {
        private AuthContext _ctx;
        private ApplicationUser _userManager;
        
       // private string userID;
        private DepartmentController()
        {
            _ctx = new AuthContext();
            UserController userController = new UserController();
            _userManager = userController.GetUser();
            
        }
        [Authorize/*(Roles = "user")*/]
        [Route("")]
        public IHttpActionResult Post(DepartmentModel deparment)
        {
            try
            {
                if (!ModelState.IsValid || deparment == null)
                {
                    return BadRequest(ModelState);
                }
                var checkDepartment = _ctx.Departments.Find(deparment.ID);
                if(checkDepartment != null)
                {
                    return this.Put(checkDepartment.ID, deparment);
                }
                var newDepartment = _ctx.Departments.Add(deparment);
                _ctx.SaveChanges();

                if (newDepartment == null)
                {
                    return BadRequest(ModelState);
                }

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // New Department Method
        public IHttpActionResult Put(int id, DepartmentModel deparment)
        {
            try
            {
                if (deparment == null) return BadRequest("Could not read department from body");

                var originalDepartment = _ctx.Departments.Find(id);

                if (originalDepartment == null || originalDepartment.ID != id)
                {
                    return BadRequest("department is not found");
                }
                else
                {
                    deparment.ID = id;
                }
                _ctx.Entry(originalDepartment).CurrentValues.SetValues(deparment);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Update Department Method
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var originalDepartment = _ctx.Departments.Find(id);

                if (originalDepartment == null || originalDepartment.ID != id)
                {
                    return BadRequest("department is not found");
                }
                _ctx.Departments.Remove(originalDepartment);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Delete Department Method
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                List<DepartmentModel> departments = new List<DepartmentModel>();
                departments = _ctx.Departments.ToList();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Departments list Method
        public IHttpActionResult Get(int id)
        {
            try
            {
                List<DepartmentModel> departments = new List<DepartmentModel>();               
                departments = _ctx.Departments.Where(w => w.ID == id).ToList();                
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Department Method
    }
}
