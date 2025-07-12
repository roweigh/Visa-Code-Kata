using JobApplicationApi.DTO;
using JobApplicationApi.Models;
using JobApplicationApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationApi.Controllers
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
        public async Task<ActionResult<int>> CheckoutProducts()
        {
            var cost = await _service.CheckoutProducts();
            return Ok(cost);
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
                if (!_service.JobApplicationExists(id))
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
