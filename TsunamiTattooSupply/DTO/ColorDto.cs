using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class ColorDto
	{
	 
		public int ID { get; set; }
		 
		public string? Code { get; set; }
		 
		public string Name { get; set; }
		 
		public bool IsCustom { get; set; }

		public string StatusID { get; set; }

		public string StatusDescription { get; set; }

		public string StatusColor { get; set; }

	}
}
