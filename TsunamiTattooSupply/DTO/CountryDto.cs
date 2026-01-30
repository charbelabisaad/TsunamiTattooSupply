using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class CountryDto
	{
	 
		public int ID { get; set; }
 
		public string ISO2 { get; set; }
		 
		public string ISO3 { get; set; }
		 
		public string Name { get; set; }
		 
		public string? Code { get; set; }
		 
		public decimal ShippingCost { get; set; }
		 
		public bool ShippingCostFixed { get; set; } 
		 
		public bool Sales { get; set; } 
		 
		public double TotalRange { get; set; } 
		 
		public double MoneyTransferFees { get; set; } 
		 
		public string IP { get; set; } 
		 
		public bool Native { get; set; } 
 
		public string? Flag { get; set; }
		 
	}
}
