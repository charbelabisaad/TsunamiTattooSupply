using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class ProductTypeDto
	{ 
		public int ID { get; set; }
		 
		public string Description { get; set; }
		 
		public bool ShowFront { get; set; } = true;
 
		public int Rank { get; set; }
		 
		public string StatusID { get; set; }

		public string StatusDescription { get; set; }

		public string StatusColor { get; set; }


	}
}
