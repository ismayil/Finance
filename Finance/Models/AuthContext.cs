using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Finance.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int DepartmentCode { get; set; }      
        public List<string> Data { get; set; }        
    }

    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DateModel> Dates { get; set; }
        public DbSet<ValueModel> Values { get; set; }
        public DbSet<TitleModel> Titles { get; set; }
        public DbSet<LockStatusModel> LockStatus { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<DepartmentModel>().ToTable("Department");
            modelBuilder.Entity<DateModel>().ToTable("Date");
            modelBuilder.Entity<ValueModel>().ToTable("Value");
            modelBuilder.Entity<TitleModel>().ToTable("Title");
            modelBuilder.Entity<LockStatusModel>().ToTable("LockStatus");

        }
        public AuthContext()
            : base("AuthContext", throwIfV1Schema: false)
        {
        }
        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }
}