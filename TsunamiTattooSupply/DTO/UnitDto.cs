using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class UnitDto
	{ 
		public int ID { get; set; }
		 
		public string ShortDescription { get; set; }
		 
		public string LongDescription { get; set; }
		 
		public string StatusID { get; set; }

		public string StatusDescription { get; set; }

		public string StatusColor { get; set; }

	}
}
