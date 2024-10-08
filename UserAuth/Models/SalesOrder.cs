using System.ComponentModel.DataAnnotations;

namespace UserAuth.Models
{
    public class SalesOrder
    {
        [Key]
        public int SalesId { get; set; }

        // Product-related fields (read-only price from Product table)
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; } // This will be set from Product table
    }
}
