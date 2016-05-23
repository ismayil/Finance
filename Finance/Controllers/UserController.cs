using Finance.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Finance.Controllers
{
    public class UserController:ApiController
    {
        private AuthContext _ctx;
        private ApplicationUser _userManager;
        public UserController()
        {
            _ctx = new AuthContext();
        }
        public ApplicationUser GetUser()
        {
            try
            {
                string userIdValue = "";
                List<string> userRoles = new List<string>();
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims.ToList();

                    if (userIdClaim != null)
                    {
                        foreach (var claim in userIdClaim)
                        {
                            if(claim.Type == ClaimTypes.NameIdentifier) userIdValue = claim.Value;
                            if (claim.Type == ClaimTypes.Role) userRoles.Add(claim.Value);
                        }
                       
                    }
                }
                _userManager = _ctx.Users.Find(userIdValue);
                _userManager.Data = userRoles;
                
            }
            catch (Exception ex)
            {

            }
            return _userManager;
        }
    }
}