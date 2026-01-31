using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class SpecDto
	{ 
		public int ID { get; set; }
		 
		public string Description { get; set; }
		 
		public string StatusID { get; set; }

		public string StatusDescription { get; set; }

		public string StatusColor { get; set; }
		  
	}
}
