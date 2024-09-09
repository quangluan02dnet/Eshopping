using System.ComponentModel.DataAnnotations;

namespace Shopping_tutorial.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please enter your name")]
		public string UserName { get; set; }
		
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Please enter your password")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
