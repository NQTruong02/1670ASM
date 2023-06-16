using System.ComponentModel.DataAnnotations;

namespace ASM_Bicycle_Shops.Models
{
    public class CategoryProduct
    {
        [Key]
        public int CategoryProductId { get; set; }
        [Required]
        public string CategoryProductName { get; set; }
        public string? CategoryProductDecription { get; set; } = string.Empty;
    }
}
