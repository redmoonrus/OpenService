using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }

    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string system_type { get; set; }
        public string order_number { get; set; }
        public string source_order { get; set; }
        public string converted_order { get; set; }
        public DateTime created_at { get; set; }
    }
}
