using System.ComponentModel.DataAnnotations;

namespace TShirtEmpAdmin.Models
{
    public class ShirtSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Visible to Users")]
        public bool? Active { get; set; }
        public bool IsSelected { get; set; }

    }
}