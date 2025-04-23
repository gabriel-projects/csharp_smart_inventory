using Microsoft.AspNetCore.Mvc;

namespace Api.GRRInnovations.SmartInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetProducts()
        {
            // Simulate fetching products from a database or service
            var products = new List<string> { "Product1", "Product2", "Product3" };
            return Ok(products);
        }
    }
}
