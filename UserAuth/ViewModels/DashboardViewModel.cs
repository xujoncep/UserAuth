namespace UserAuth.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalInventoryValue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public List<CategoryProductCount> CategoryProductCounts { get; set; }
    }

    public class CategoryProductCount
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }
}
