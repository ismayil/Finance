using Finance.Helpers;
using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Finance.Controllers
{
    [RoutePrefix("api/Lock")]
    public class LockController : ApiController
    {
        private AuthContext _ctx;
        private ApplicationUser _userManager;

        // private string userID;
        private LockController()
        {
            _ctx = new AuthContext();
            UserController userController = new UserController();
            _userManager = userController.GetUser();

        }
        // [Authorize(Roles = "Administrator")]
        [Route("")]
        public IHttpActionResult Post(LockStatusModel Lock)
        {
            try
            {
                if (!ModelState.IsValid || Lock == null)
                {
                    return BadRequest(ModelState);
                }
                var oldLock = _ctx.LockStatus.SingleOrDefault(w =>
                                                 w.DepartmentCode == Lock.DepartmentCode
                                                 && w.DateId == Lock.DateId
                                                );
                if (oldLock != null)
                {
                    return Put(oldLock.ID, Lock);
                }
                var newLock = _ctx.LockStatus.Add(Lock);
                _ctx.SaveChanges();

                if (newLock == null)
                {
                    return BadRequest(ModelState);
                }

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // New Lock Method
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public IHttpActionResult Put(int id, LockStatusModel Lock)
        {
            try
            {
                if (Lock == null) return BadRequest("Could not read Lock from body");

                var originalLock = _ctx.LockStatus.Find(id);

                if (originalLock == null || originalLock.ID != id)
                {
                    return BadRequest("Lock is not found");
                }
                else
                {
                    Lock.ID = id;
                }
                _ctx.Entry(originalLock).CurrentValues.SetValues(Lock);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // UpLock Lock Method
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var originalLock = _ctx.LockStatus.Find(id);

                if (originalLock == null || originalLock.ID != id)
                {
                    return BadRequest("Lock is not found");
                }
                _ctx.LockStatus.Remove(originalLock);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Delete Lock Method
        [Route("")]
        [Authorize(Roles = "Administrator")]
        public IHttpActionResult Get()
        {
            try
            {

                //List<LockStatusModel> Locks = new List<LockStatusModel>();
                var Locks = _ctx.LockStatus
                        .Join(_ctx.Departments,
                              locks => locks.DepartmentCode,
                              deps => deps.DepartmentCode,
                              (locks, deps) => new { locks, deps })
                         .Join(_ctx.Dates,
                              locks => locks.locks.DateId,
                              dat => dat.ID,
                              (locks, dat) => new
                              {
                                  id = locks.locks.ID,
                                  localStatus = locks.locks.LocalStatus,
                                  remoteStatus = locks.locks.RemoteStatus,
                                  departmentCode = locks.locks.DepartmentCode,
                                  dateId = locks.locks.DateId,
                                  departmentName = locks.deps.DepartmentName,
                                  dates = dat.Dates
                              }).Where(w=>w.localStatus==true)                              
                              .ToList();
                return Ok(Locks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Lock Method

        [HttpPost]
        [Route("current")]
        public IHttpActionResult Post(LockGetHelper Lock)
        {
            try
            {
                if (!ModelState.IsValid || Lock == null)
                {
                    return BadRequest(ModelState);
                }
                var oldLock = _ctx.LockStatus.SingleOrDefault(w =>
                                                 w.DepartmentCode == Lock.DepartmentCode
                                                 && w.DateId == Lock.DateId
                                                );

                return Ok(oldLock);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // New Lock Method
    }
}
