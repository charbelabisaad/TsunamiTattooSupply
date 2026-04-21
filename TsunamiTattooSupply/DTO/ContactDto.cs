using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class ContactDto
	{ 
		public string ID { get; set; }
		 
		public string Location { get; set; }
		 
		public string LocationMapLink { get; set; }
		 
		public string Phone { get; set; }
		 
		public string Email { get; set; }

	}
}
