using System;
using System.Collections;
using System.Collections.Generic;
using Models;

namespace WebApplication
{
    public class OrderRequest
    {
        public string orderNumber { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime createdAt { get; set; }
    }

    
}