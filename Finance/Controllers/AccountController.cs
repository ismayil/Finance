using Microsoft.AspNet.Identity;
using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Finance.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;
        private AuthContext _ctx;

        public AccountController()
        {
            _repo = new AuthRepository();
            _ctx = new AuthContext();
        }                        
        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }
        
        [Route("")]
        [Authorize(Roles ="Administrator")]
        public IHttpActionResult Get() //todo this not valid
        {
            try
            {
                var users = _ctx.Users
                            .GroupJoin(_ctx.Departments,
                                    u => u.DepartmentCode,
                                    d => d.DepartmentCode,
                                    (u, d) => new { u, d = d.DefaultIfEmpty() })
                                    // Each pair may have multiple os here
                                    .SelectMany(s => s.d.Select(o => new { s.u, o }))
                                    // Flattened, but o may be null
                                    .Select(pair => new
                                    {
                                        UserName = pair.u.UserName,
                                        id = pair.u.Id,
                                        DepartmentName = pair.o.DepartmentName != null ? pair.o.DepartmentName : pair.u.DepartmentCode.ToString()
                                    });
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public IHttpActionResult Delete(string id) //todo this not valid
        {
            try
            {
                //if(_repo.FindUser())
                var user = _ctx.Users.Find(id);
                _ctx.Users.Remove(user);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string id,UserPutModel user) //todo this not valid
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            IdentityResult result = await _repo.UpdateUser(user,id);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
