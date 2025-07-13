using CheckoutApi.DTO;
using CheckoutApi.Models;
using CheckoutApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckoutApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var response = await _service.GetProducts();
            return Ok(response);
        }

        // POST: api/products/checkout
        [HttpPost("checkout")]
        public async Task<ActionResult<int>> CheckoutProducts([FromBody] string basket)
        {
            var response = await _service.CheckoutProducts(basket);
            return Ok(response);
        }


        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var application = await _service.GetProduct(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                await _service.UpdateProduct(id, ProductMapper.ToDTO(product));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            var dto = ProductMapper.ToDTO(product);
            var newProduct = await _service.AddProduct(dto);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }
    }
}
