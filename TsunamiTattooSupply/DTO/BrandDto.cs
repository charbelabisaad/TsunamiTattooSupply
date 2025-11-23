using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class BrandDto
	{
	 
		public int ID { get; set; }
		 
		public string Name { get; set; }
		 
		public string? Summary { get; set; }
		 
		public string? Image { get; set; }
		 
		public bool ShowHome { get; set; } = true;
		 
		public int Rank { get; set; } = 0;
 
		public string StatusID { get; set; }

		public string Status {  get; set; }

		public string StatusColor { get; set; }
		 
	}
}
