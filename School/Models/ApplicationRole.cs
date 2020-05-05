using Microsoft.AspNetCore.Identity;

namespace School.Models
{
	public class ApplicationRole : IdentityRole
	{
		public string Description { get; set; }
	}
}