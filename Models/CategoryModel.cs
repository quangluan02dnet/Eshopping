using System.ComponentModel.DataAnnotations;

namespace Shopping_tutorial.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter category's name !")]
        public string Name { get; set; }
		[Required (ErrorMessage = "Please enter description !")]
		public string Description { get; set; }
		public string Slug { get; set; }

        public int Status { get; set; }

    }
}
