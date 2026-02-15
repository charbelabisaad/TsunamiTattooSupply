using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class PromoCodeDto
	{
		public int ID { get; set; }
		 
		public string Code { get; set; }
 
		public string Title { get; set; }
		 
		public string Description { get; set; }
		 
		public int Percentage { get; set; }
 
		public string Level { get; set; }
		 
		public DateTime ExpiryDate { get; set; }
 
		public int ClientID { get; set; }

		public string ClientName { get; set; }
 
		public string StatusID { get; set; }

		public string StatusDescription { get; set; }

		public string StatusColor { get; set; }

	}
}
