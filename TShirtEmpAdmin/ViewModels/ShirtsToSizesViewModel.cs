using System.Collections.Generic;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.ViewModels
{
    public class ShirtsToSizesViewModel
    {
        public int ShirtId { get; set; }
        public IEnumerable<Shirt> Shirts { get; set; }
        public string ShirtCause { get; set; }
        public IEnumerable<ShirtSize> ShirtSizes { get; set; }
        public int FileId { get; set; }
        public IEnumerable<File> Files { get; set; }
        public int ShirtToSizeId { get; set; }
        public bool IsSelected { get; set; }
        public IEnumerable<ShirtToSize> ShirtToSizes { get; set; }

    }
}