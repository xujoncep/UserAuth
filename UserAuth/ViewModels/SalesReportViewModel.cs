namespace UserAuth.ViewModels
{
    public class SalesReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<SalesInfo> Sales { get; set; }
        public decimal TotalSalesValue { get; set; }
    }

    public class SalesInfo
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice => Price * Quantity; // Calculating total price for each sale
    }

}
