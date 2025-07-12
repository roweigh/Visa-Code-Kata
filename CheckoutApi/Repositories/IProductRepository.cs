using JobApplicationApi.Models;

namespace JobApplicationApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> Get(int id);
        Task Post(Product product);
        Task Put(int id, Product product);
        bool Exists(int id);
    }
}
