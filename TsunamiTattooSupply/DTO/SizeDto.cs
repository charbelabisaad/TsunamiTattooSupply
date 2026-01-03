using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class SizeDto
	{
	 
		public int ID { get; set; }
		 
		public string Description { get; set; }
		 
		public int Rank { get; set; } = 0;
		 
		public string StatusID { get; set; }

		public string StatusDescription { get; set; }

		public string StatusColor { get; set; }

	}
}
