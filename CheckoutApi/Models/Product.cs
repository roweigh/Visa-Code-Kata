using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckoutApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public required string ProductName { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public required double UnitPrice { get; set; }

        [Column(TypeName = "int")]
        public int? DiscountQuantity { get; set; } // Quantity needed to trigger DiscountedPrice

        [Column(TypeName = "decimal(10,2)")]
        public double? DiscountPrice { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DiscountStartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DiscountEndDate { get; set; }
    }

    public class ScannedProduct
    {
        public int Quantity { get; set; }
        public required Product Product { get; set; }
    }
}