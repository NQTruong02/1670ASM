using ASM_Bicycle_Shops.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASM_AppWeb_Bicycle_Shop.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }
        public string? EmpPhotoPath { get; set; }
        public string? EmpFileName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please choose Img")]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public CartItem()
        {
        }

        public CartItem(Product product)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Price = product.ProductPrice;
            Quantity = 1;
            EmpPhotoPath = product.EmpPhotoPath;
            EmpFileName = product.EmpFileName;
            ImageFile = product.ImageFile;
        }

    }
}
