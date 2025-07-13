using CheckoutApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckoutApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDBContext _context;

        public ProductRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            IQueryable<Product> items = _context.Product;
            return await Task.FromResult(items);
        }

        public async Task<Product> Get(int Id)
        {
            return await _context.Product.FindAsync(Id);
        }

        public async Task Post(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task Put(int id, Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool Exists(int Id)
        {
            return _context.Product.Any(e => e.Id == Id);
        }
    }
}
