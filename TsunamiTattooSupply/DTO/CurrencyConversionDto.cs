using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class CurrencyConversionDto
	{ 
		public int ID { get; set; }
 
		public int CurrencyIDFrom { get; set; }
		 
		public int CurrencyIDTo { get; set; }
		 
		public string Operator { get; set; }
 
		public decimal Rate { get; set; }
	}
}
