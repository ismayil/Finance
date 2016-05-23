using Finance.Helpers;
using Finance.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Finance.Controllers
{
    [RoutePrefix("api/Value")]
    public class ValueController : ApiController
    {
        private AuthContext _ctx;
        private ApplicationUser _userManager;

        // private string userID;
        private ValueController()
        {
            _ctx = new AuthContext();
            UserController userController = new UserController();
            _userManager = userController.GetUser();

        }
        [Authorize/*(Roles = "user")*/]
        [Route("")]
        public IHttpActionResult Post(ValuePostHelper Value)
        {
            try
            {
                if (!ModelState.IsValid || Value == null)
                {
                    return BadRequest(ModelState);
                }

                if (Value.DepartmentCode != _userManager.DepartmentCode && !User.IsInRole("Administrator"))
                {
                    return BadRequest("You don't have access to this proccess");
                }

                foreach (var val in Value.ValueList)
                {
                    var addValue = new ValueModel
                    {
                        DepartmentCode = Value.DepartmentCode,
                        TitleCode = val.TitleCode,
                        DateId = Value.DateId,
                        DebitKredit = Value.DebitKredit,
                        Value = val.Value
                    };
                    var newValue = _ctx.Values.Add(addValue);
                }
                _ctx.SaveChanges();

                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // New Value Method
        public IHttpActionResult Put(int id, ValueModel Value)
        {
            try
            {
                if (Value == null) return BadRequest("Could not read Value from body");

                var originalValue = _ctx.Values.Find(id);

                if (originalValue == null || originalValue.ID != id)
                {
                    return BadRequest("Value is not found");
                }
                else
                {
                    Value.ID = id;
                }
                _ctx.Entry(originalValue).CurrentValues.SetValues(Value);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Update Value Method
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var originalValue = _ctx.Values.Find(id);

                if (originalValue == null || originalValue.ID != id)
                {
                    return BadRequest("Value is not found");
                }
                _ctx.Values.Remove(originalValue);
                _ctx.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Delete Value Method
        public IHttpActionResult Get(int id)
        {
            try
            {
                List<ValueModel> Values = new List<ValueModel>();
                Values = _ctx.Values.Where(w =>
                                            w.ID == id
                                            ).ToList();
                return Ok(Values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Value Method

        [Route("Get")]
        [Authorize]
        public IHttpActionResult Post(ValuesGetHelper value)
        {
            try
            {
                if (value.DepartmentCode != _userManager.DepartmentCode && !User.IsInRole("Administrator"))
                {
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "You don't have access to this proccess" };
                    throw new HttpResponseException(msg);
                }
                var fullValues = _ctx.Values
                                        .Join(
                                            _ctx.Titles,
                                            values => values.TitleCode,
                                            title => title.ID,
                                            (values, title) => new { values, title }
                                        )
                                        .Join(
                                            _ctx.Dates,
                                            values => values.values.DateId,
                                            date => date.ID,
                                            (values, date) => new
                                            {
                                                id = values.values.ID,
                                                departmentCode = values.values.DepartmentCode,
                                                debitKredit = values.values.DebitKredit,
                                                title = values.title.Title,
                                                titlecode = values.title.ID,
                                                description = values.title.Description,
                                                value = values.values.Value,
                                                date = date.Dates,
                                                dateName = date.Description,
                                                dateId = date.ID
                                            }
                                        )
                                        .Where(w => w.dateId == value.DateId && w.departmentCode == value.DepartmentCode);
                var _debit = fullValues.Where(w => w.debitKredit == 250);
                var _kredit = fullValues.Where(w => w.debitKredit == 630);
                var _lockStatusCheck = _ctx.LockStatus.SingleOrDefault(s => s.DepartmentCode == value.DepartmentCode && s.DateId == value.DateId);
                var _lockStatus = _lockStatusCheck != null ? new { local = _lockStatusCheck.LocalStatus, remote = _lockStatusCheck.RemoteStatus } : new { local = false, remote = false };
                var result = new { debit = _debit, kredit = _kredit, lockStatus = _lockStatus };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Value Method

        [Route("total")]
        [Authorize]
        public IHttpActionResult Post(ValueRootGetHelper value)
        {
            try
            {
                var sConnection = ((SqlConnection)_ctx.Database.Connection);
                SqlCommand cmd = new SqlCommand();
                DataTable _debit = new DataTable();
                DataTable _kredit = new DataTable();
                if (sConnection != null && sConnection.State == ConnectionState.Closed)
                {
                    sConnection.Open();
                }
                using (SqlDataAdapter ad = new SqlDataAdapter())
                {
                    cmd = new SqlCommand("pr_total", sConnection);
                    cmd.Parameters.Add(new SqlParameter("@DateId", value.DateId));
                    cmd.Parameters.Add(new SqlParameter("@DebitKredit", "250"));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentCode", value.DepartmentCode));
                    cmd.CommandType = CommandType.StoredProcedure;
                    ad.SelectCommand = cmd;
                    ad.Fill(_debit);
                }
                using (SqlDataAdapter ad = new SqlDataAdapter())
                {
                    cmd = new SqlCommand("pr_total", sConnection);
                    cmd.Parameters.Add(new SqlParameter("@DateId", value.DateId));
                    cmd.Parameters.Add(new SqlParameter("@DebitKredit", "630"));
                    cmd.Parameters.Add(new SqlParameter("@DepartmentCode", value.DepartmentCode));
                    cmd.CommandType = CommandType.StoredProcedure;
                    ad.SelectCommand = cmd;
                    ad.Fill(_kredit);
                }
                var result = new { debit = _debit, kredit = _kredit };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } // Get Value Method
        public class TotalValue
        {
            string test { get; set; }
        };
    }
}
