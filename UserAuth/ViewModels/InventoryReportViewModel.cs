using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UserAuth.ViewModels
{
    public class InventoryReportViewModel
    {
        public string InventoryName { get; set; }
        public DateTime ReportGeneratedTime { get; set; }
        public List<ProductInfo> Products { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }
    public class ProductInfo
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity; // Calculate total price for each product
    }
}
