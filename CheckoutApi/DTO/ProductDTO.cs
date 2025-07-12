namespace JobApplicationApi.DTO
{
    public class ProductDTO
    {
        public required int Id { get; set; }
        public required string ProductName { get; set; }
        public required double UnitPrice { get; set; }
        public int? DiscountQuantity { get; set; }
        public double? DiscountPrice { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
    }
}