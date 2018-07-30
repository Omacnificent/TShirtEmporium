using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TShirtEmpAdmin.Models
{
    public class ShirtToSize
    {
        //Defining Composite Key
        [Key, ForeignKey("Shirt"), Column(Order = 0)]
        public int ShirtId { get; set; }
        public virtual Shirt Shirt { get; set; }
        public virtual ShirtSize ShirtSize { get; set; }
        [Key, ForeignKey("ShirtSize"), Column(Order = 1)]
        public int ShirtSizeId { get; set; }
    }
}