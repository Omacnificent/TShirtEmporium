using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TShirtEmpAdmin.Models
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext()
            : base("name=UserConnection", throwIfV1Schema: false)
        {
        }

        public static UserDbContext Create()
        {
            return new UserDbContext();
        }
    }
}