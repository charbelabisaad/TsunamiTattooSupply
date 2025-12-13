using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class CurrencyDto
	{ 
		public int ID { get; set; }
 
		public string Code { get; set; }
		 
		public string Description { get; set; }
		 
		public string Symbol { get; set; }

		public string Priority { get; set; }

		public int? CountryID { get; set; }

	}
}
