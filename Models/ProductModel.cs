using Shopping_tutorial.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_tutorial.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
		[Required(ErrorMessage = "Please enter name !")]
        [MinLength(1)]
		public string Name { get; set; }
        public string Slug { get; set; }

        [Required (ErrorMessage = "Please enter description !")]
        [MinLength(1)]
        public string Description { get; set; }

		[Required(ErrorMessage = "Please enter price !")]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName ="decimal(15, 2)")]
		public decimal Price { get; set; }


        [Required, Range(1, int.MaxValue, ErrorMessage = "Please choose a brand !")]
        public int BrandId {  get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please choose a category !")]

        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
