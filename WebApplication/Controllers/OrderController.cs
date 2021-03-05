using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext _context;
        
        public OrderController(OrderContext context)
        {
            _context = context;
        }

        [HttpPost("{system}")]
        public IActionResult Post(string system,[FromBody]OrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            Order order = new Order()
            {
                system_type = system,
                order_number = request.orderNumber,
                created_at = request.createdAt,
                source_order = JsonSerializer.Serialize(request.Products)
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return Created("", request);
        }

        public IActionResult Get()
        {

            return Ok(_context.Orders.ToList());
        }
    }
}