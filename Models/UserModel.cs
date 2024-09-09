using System.ComponentModel.DataAnnotations;

namespace Shopping_tutorial.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please enter your name")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Please enter your email"), EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Please enter your password")]
		public string Password { get; set; }
	}
}
