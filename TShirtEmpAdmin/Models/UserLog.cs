using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace TShirtEmpAdmin.Models
{
    public class UserLog
    {
        public int Id { get; set; }
        public ApplicationUser Customer { get; set; }
        public DateTime LogDate { get; set; }
    }
}