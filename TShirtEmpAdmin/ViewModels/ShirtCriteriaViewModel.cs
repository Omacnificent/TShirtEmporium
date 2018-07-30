using System;

namespace TShirtEmpAdmin.ViewModels
{
    public class ShirtCriteriaViewModel
    {
        public string ShirtCause { get; set; }
        public DateTime CreateDate { get; set; }
        public int ShirtSizeId { get; set; }
        public int ShirtQuantityId { get; set; }
        public int DeliveryOptionId { get; set; }
        public decimal Total { get; set; }
        public string ShirtYear { get; set; }
        public bool? OrderCompleted { get; set; }
        public bool? RecievedShirt { get; set; }
    }
}