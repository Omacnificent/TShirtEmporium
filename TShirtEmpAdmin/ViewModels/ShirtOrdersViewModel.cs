using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirtEmpAdmin.Models;


namespace TShirtEmpAdmin.ViewModels
{
    public class ShirtOrdersViewModel
    {
        public int ShirtSizeId { get; set; }
        public IEnumerable<ShirtSize> ShirtSizes { get; set; }
        public int ShirtQuantityId { get; set; }
        public IEnumerable<ShirtQuantity> ShirtQuantities { get; set; }
        public int DeliveryOptionId { get; set; }
        public IEnumerable<DeliveryOption> DeliveryOptions { get; set; }
        public int ShirtId { get; set; }
        public IEnumerable<Shirt> Shirts { get; set; }
        public int FileId { get; set; }
        public IEnumerable<File> Files { get; set; }
        public int OrderId { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public decimal Total { get; set; }
    }
}