namespace UserAuth.ViewModels
{
    public class PurchaseReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<PurchaseInfo> Purchases { get; set; }
        public decimal TotalPurchaseValue { get; set; }
    }

    public class PurchaseInfo
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice => Price * Quantity; // Calculating total price for each purchase
    }

}
