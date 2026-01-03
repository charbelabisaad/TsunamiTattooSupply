using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class RoleDto
	{
		public int ID { get; set; }
		 
		public string Description { get; set; }
		 
		public string StatusID { get; set; }

		public string Status { get; set; }
		 
		public bool IsAdmin { get; set; } 

		public string StatusColor { get; set; }

	}
}
