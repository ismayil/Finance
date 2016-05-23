using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;

namespace Finance.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private string userID;
        private IList<string> roles;
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            ApplicationUser _user = new ApplicationUser();
            using (AuthRepository _repo = new AuthRepository())
            {
                ApplicationUser user = await _repo.FindUser(context.UserName, context.Password);
               
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                _user = user;
                roles = _repo.UserRoles(user.Id);
                userID = user.Id;
            }

            var props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            {
                                "user_name", _user.UserName
                            },
                            {
                                "departmentCode", _user.DepartmentCode.ToString()
                            }
                        });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userID));
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //List<KeyValuePair<string, string>> properties = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("test","test")};
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}