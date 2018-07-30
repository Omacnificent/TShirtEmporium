using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TShirtEmpAdmin.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}