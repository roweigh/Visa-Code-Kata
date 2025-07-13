using CheckoutApi.DTO;
using CheckoutApi.Models;
using CheckoutApi.Repositories;

namespace CheckoutApi.Service
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        private readonly IProductRepository _repository = repository;

        // Generic CRUD operations
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = await _repository.GetAll();
            return products
                .Select(ProductMapper.ToDTO)
                .ToList();
        }

        public async Task<ProductDTO> GetProduct(int Id)
        {
            var product = await _repository.Get(Id);
            return ProductMapper.ToDTO(product);
        }

        public async Task UpdateProduct(int id, ProductDTO dto)
        {
            var entity = ProductMapper.ToEntity(dto);
            await _repository.Put(id, entity);
        }

        public async Task<Product> AddProduct(ProductDTO dto)
        {
            var entity = ProductMapper.ToEntity(dto);
            await _repository.Post(entity);
            return entity;
        }

        public bool ProductExists(int id)
        {
            return _repository.Exists(id);
        }

        // Generates total cost of basket based on existence of discounts for each basketed product
        public async Task<double> CheckoutProducts(string basket)
        {
            if (string.IsNullOrWhiteSpace(basket))
                return 0;

            double total = 0;
            IEnumerable<Product> products = await _repository.GetAll();
            Dictionary<string, ScannedProduct> productMap = products.ToDictionary(
                product => product.ProductName, 
                product => new ScannedProduct
                {
                    Quantity = 0,
                    Product = product
                });

            // Count products in basket
            // Ignores any invalid/unrecognised products
            foreach (char ch in basket)
            {
                string productName = ch.ToString();
                if (productMap.ContainsKey(productName))
                {
                    productMap[productName].Quantity += 1;
                }
            }

            // Apply discounts for each product after loading basket
            foreach (KeyValuePair<string, ScannedProduct> entry in productMap)
            {
                Product product = entry.Value.Product;
                int quantity = entry.Value.Quantity;
                total += CalculateProductCost(product, quantity);
            }

            return Math.Round(total, 2);
        }

        private double CalculateProductCost(Product product, int quantity)
        {
            if (quantity <= 0) return 0;

            if (product.DiscountQuantity.HasValue && product.DiscountPrice.HasValue)
            {
                int discountQuantity = product.DiscountQuantity.Value;
                double discountPrice = product.DiscountPrice.Value;

                int discountedSets = quantity / discountQuantity;
                int remainder = quantity % discountQuantity;

                return (discountedSets * discountPrice) + (remainder * product.UnitPrice);
            } else
            {
                return quantity * product.UnitPrice;
            }
        }
    }
}
