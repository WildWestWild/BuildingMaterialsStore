﻿using System.Collections.Generic;

namespace BuildingMaterialsStore.Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; }
        
        public IEnumerable<Category> Categories { get; set; }
    }
}