using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Shopping_tutorial.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation { get; set; }

		public string RoleId { get; set; }

		[ForeignKey("RoleId")]
		public IdentityRole Role { get; set; }
	}
}
