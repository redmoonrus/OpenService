using Models;

namespace Executer
{
    public class zomato
    {
        public Product Execute(Product value)
        {
            value.paidPrice = value.paidPrice / value.quantity;
            return value;
        }
    }
}