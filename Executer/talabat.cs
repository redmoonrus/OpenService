using System;
using Models;

namespace Executer
{
    public class talabat
    {
        public Product Execute(Product value)
        {
            value.paidPrice = value.paidPrice > 0 ? -value.paidPrice : value.paidPrice;
            return value;
        }
    }
    
}