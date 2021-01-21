using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildingMaterialsStore.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        
        public IEnumerable<SelectListItem> ProductListItem { get; set; }
        
        public IEnumerable<SelectListItem> ApplicationListItem { get; set; }
    }
}