using JobApplicationApi.DTO;
using JobApplicationApi.Models;
using JobApplicationApi.Repositories;

namespace JobApplicationApi.Service
{
    public class ProductService(IProductRepository repository) : IProductService
    {
        private readonly IProductRepository _repository = repository;

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

        public async Task<double> CheckoutProducts()
        {
            var scannedProductMap = new Dictionary<string, ScannedProduct>();

            var input = "AAABABBAA";


            // Count number of products scanned
            foreach (var product in input.ToCharArray()) // Split into array of chars?
            {
                string productId = product.ToString();
                if (!scannedProductMap.ContainsKey(productId))
                {

                    // DEBUG
                    if (productId == "A")
                    {
                        scannedProductMap[productId] = new ScannedProduct
                        {
                            Quantity = 1,
                            Product = new Product
                            {
                                Id = 1,
                                ProductName = "A",
                                UnitPrice = 0.5,
                                DiscountQuantity = 3,
                                DiscountPrice = 1
                            }
                        };
                    }
                    else if (productId == "B")
                    {
                        scannedProductMap[productId] = new ScannedProduct
                        {
                            Quantity = 1,
                            Product = new Product
                            {
                                Id = 2,
                                ProductName = "B",
                                UnitPrice = 2
                            }
                        };
                    }
                    // DEBUG OVER

                    // AWAIT GET PRODUCT BY ID

                } else
                {
                    scannedProductMap[productId].Quantity += 1;
                }
     
            }

            var result = 0;

            // PRINTING DEBUG
            foreach (var kvp in scannedProductMap)
            {
                string key = kvp.Key;
                ScannedProduct entry = kvp.Value;
                Product product = entry.Product;

                Console.WriteLine($"Key: {key}");
                Console.WriteLine($"  Scanned: {entry.Quantity}");
                Console.WriteLine($"  Id: {product.Id}");
                Console.WriteLine($"  UnitCost: {product.UnitPrice}");

                if (product.DiscountQuantity != null)
                {
                    Console.WriteLine($"  Discount - Amount: {product.DiscountQuantity}, Cost: {product.DiscountPrice}");

                    var discounted = (int)Math.Floor((double)entry.Quantity / (double)product.DiscountQuantity);
                    var undiscounted = (entry.Quantity % product.DiscountQuantity) * product.UnitPrice;
                    Console.WriteLine($"  Discounted Cost: {discounted}, Undiscounted Cost: {undiscounted}");
                    //result += Math.Floor(entry.scanned / product.discount.amount) * product.discount.cost;
                }
                else
                {
                    Console.WriteLine("  Discount: None");
                }


            }

            return result;
        }

        public bool JobApplicationExists(int id)
        {
            return _repository.Exists(id);
        }
    }
}
