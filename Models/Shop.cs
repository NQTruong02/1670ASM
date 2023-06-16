using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASM_AppWeb_Bicycle_Shop.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public DateTime Purchase_date { get; set; }
        [Required]
        public decimal TotalRevenue
        {
            get { return Quantity * ProductPrice; }
        }
    }
}
