using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TShirtEmpAdmin.Models
{
    public class ShirtQuantity
    {
        public int Id { get; set; }
        public int Name { get; set; }
        [Display(Name = "Visible to Users")]
        public bool? Active { get; set; }
    }
}