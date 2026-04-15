using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class ClientsDto
	{
	 
		public int ID { get; set; }
 
		public string Name { get; set; }
   
		public string Email { get; set; }
 
		public int PhoneCountryID { get; set; }
		 
		public string PhoneCountryCode { get; set; }
 
		public string PhoneNumber { get; set; }

	}
}
