using System;

namespace TShirtEmpAdmin.Models
{
    public class Order
    {
        public int Id { get; set; }
        public ApplicationUser Customer { get; set; }
        public string ShirtCause {get; set;}
        public DateTime CreateDate { get; set; }
        public ShirtSize ShirtSize { get; set; }
        public ShirtQuantity ShirtQuantity { get; set; }
        public DeliveryOption DeliveryOption { get; set; } 
        public string OShirtSize { get; set; }
        public int OShirtQuantity { get; set; }
        public string ODeliveryOption { get; set; }
        public decimal Total { get; set; }
        public string ShirtYear { get; set; }
        public int OrderNum { get; set; }
        public bool? OrderCompleted { get; set; }
        public bool? RecievedShirt { get; set; }
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ShirtId { get; set; }
        public int FileId { get; set; }        
    }
}