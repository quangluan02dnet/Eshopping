using System.ComponentModel.DataAnnotations;

namespace Shopping_tutorial.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage ="Please enter Brand !")]
        public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Please enter Description !")]
		public string Description { get; set; }
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
