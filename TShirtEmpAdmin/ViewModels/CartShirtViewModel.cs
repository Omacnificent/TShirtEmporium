using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirtEmpAdmin.ViewModels;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.ViewModels
{
    public class CartShirtViewModel
    {
        public ShirtOrdersViewModel ShirtOrdersViewModel { get; set; }
        public Order Order { get; set; }
    }
}