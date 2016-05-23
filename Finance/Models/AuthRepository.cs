using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Finance.Models
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;
        private UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName,
                DepartmentCode = 99
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);           
            return result;
        }
        public async Task<IdentityResult> UpdateUser(UserPutModel userModel,string id)
        {
            var oldUser = _ctx.Users.Find(id);
            oldUser.UserName = userModel.UserName;
            oldUser.DepartmentCode = userModel.DepartmentCode;
            var result = await _userManager.UpdateAsync(oldUser);
            return result;
        }
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);          
            return user;
        }
        public  IList<string> UserRoles(string userId)
        {
            IList<string> roles =  _userManager.GetRoles(userId);
            return roles;
        }
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}