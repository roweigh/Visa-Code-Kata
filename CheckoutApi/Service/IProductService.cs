using CheckoutApi.DTO;
using CheckoutApi.Models;

namespace CheckoutApi.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProduct(int Id);
        Task<Product> AddProduct(ProductDTO product);
        Task UpdateProduct(int id, ProductDTO product);
        Task<double> CheckoutProducts(string basket);
        bool ProductExists(int id);
    }
}
