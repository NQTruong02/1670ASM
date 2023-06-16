using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM_AppWeb_Bicycle_Shop.Models
{
    public class News
    {

        [Key]
        public int NewsId { get; set; }
        [Required]
        public string NewsTitle { get; set; }

        [Required]
        public string NewsAuthor { get; set; }
        [Required]

        public string NewsContent { get; set; }

        public DateTime NewsDate { get; set; }

        public string? EmpPhotoPath { get; set; }
        public string? EmpFileName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please choose Img")]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

    }
}

