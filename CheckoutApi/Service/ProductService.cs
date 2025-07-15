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


        // Generates total cost of basket based on existence of discounts for each product in basket
        public async Task<double> CheckoutProducts(string basket)
        {
            if (string.IsNullOrWhiteSpace(basket))
                return 0;

            IEnumerable<Product> products = await _repository.GetAll();
            Dictionary<string, ScannedProduct> productMap = products.ToDictionary(
                product => product.ProductName, 
                product => new ScannedProduct
                {
                    Quantity = 0,
                    Product = product
                });

            double total = 0;
            foreach (char ch in basket)
            {
                total = ScanProduct(productMap, ch);
            }

            return total;
        }

        // Scan products in basket - Ignores any invalid/unrecognised entries
        // Apply discount for each product after incrementally scanning item
        public double ScanProduct(Dictionary<string, ScannedProduct> productMap, char ch)
        {
            string productName = ch.ToString();
            if (productMap.ContainsKey(productName))
            {
                productMap[productName].Quantity += 1;
            }

            double total = 0;
            foreach (KeyValuePair<string, ScannedProduct> entry in productMap)
            {
                Product product = entry.Value.Product;
                int qty = entry.Value.Quantity;
                total += CalculateCost(product, qty);
            }

            return Math.Round(total, 2);
        }

        // Calculate cost of products scanned, applies discounts if defined
        private double CalculateCost(Product product, int qty)
        {
            if (qty <= 0) return 0;

            var activeDiscount = product.DiscountQuantity.HasValue && product.DiscountPrice.HasValue;
            if (activeDiscount) 
            {
                int discountQuantity = product.DiscountQuantity!.Value; // Guaranteed to have values as activeDiscount will be checking to enter this code block
                double discountPrice = product.DiscountPrice!.Value;

                int discountedSets = qty / discountQuantity;
                int remainder = qty % discountQuantity;

                return (discountedSets * discountPrice) + (remainder * product.UnitPrice);
            } else
            {
                return qty * product.UnitPrice;
            }
        }
    }
}
