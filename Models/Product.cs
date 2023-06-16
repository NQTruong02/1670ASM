using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_Bicycle_Shops.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        public decimal ProductPriceSale { get; set; }
        public string? ProductDecription { get; set; }
        public string? CategoryName { get; set; }
        public int? Status { get; set; }
        public string? EmpPhotoPath { get; set; }
        public string? EmpFileName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please choose Img")]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
