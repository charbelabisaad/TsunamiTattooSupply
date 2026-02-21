using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class PriceDto
	{
	 
		public int ID { get; set; }
		 
		public int ProductID { get; set; }
		 
		public int ProductTypeID { get; set; }

		public string ProductTypeDescription { get; set; }

		public int ProductDetailID { get; set; }

		public int ProductDetailDescription { get; set; }

		public int SizeID { get; set; }
		 
		public int CountryID { get; set; }
		 
		public int CurrencyID { get; set; }

		public string CurrencyCode { get; set; }

		public string CurrencyDescription { get; set; }

		public string CurrencySymbol {  get; set; }
  
		public decimal Amount { get; set; }
 
		public decimal AmountNet { get; set; }
	}
}
