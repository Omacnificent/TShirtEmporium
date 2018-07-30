using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TShirtEmpAdmin.Models
{
    public class Shirt
    {
        public int Id { get; set; }
        public string TabName { get; set; }
        public string ShirtCause { get; set; }
        public ShirtSize ShirtSizes { get; set; }
        public ShirtQuantity ShirtQuantities { get; set; }
        public decimal Price { get; set; }
        [DataType(DataType.MultilineText)]
        public string Caption { get; set; }
        public bool? SiteActive { get; set; }
        public bool? UpsizeCharge { get; set; }
        public virtual ICollection<File> Files { get; set; }
    }
}