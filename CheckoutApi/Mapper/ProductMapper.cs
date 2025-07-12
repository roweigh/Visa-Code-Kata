using JobApplicationApi.DTO;
using JobApplicationApi.Models;

public static class ProductMapper
{
    public static ProductDTO ToDTO(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice,
            DiscountQuantity = product.DiscountQuantity,
            DiscountPrice = product.DiscountPrice,
            DiscountStartDate = product.DiscountStartDate,
            DiscountEndDate = product.DiscountEndDate,
        };
    }

    public static Product ToEntity(ProductDTO dto)
    {
        return new Product
        {
            Id = dto.Id,
            ProductName = dto.ProductName,
            UnitPrice = dto.UnitPrice,
            DiscountQuantity = dto.DiscountQuantity,
            DiscountPrice = dto.DiscountPrice,  
            DiscountStartDate = dto.DiscountStartDate,
            DiscountEndDate = dto.DiscountEndDate,  
        };
    }
}