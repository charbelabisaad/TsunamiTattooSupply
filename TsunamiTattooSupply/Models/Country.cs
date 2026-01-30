using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TsunamiTattooSupply.Models
{
	[Table("Countries")]
	//[Index(nameof(Name), IsUnique = true)]
	public class Country
	{
		[Key] 
		public int ID { get; set; }

		[Required]
		[StringLength(2)]
		public string ISO2 { get; set; }

		[Required]
		[StringLength(3)]
		public string ISO3 { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[StringLength(50)]
		public string? Code { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		[Required]
		public decimal ShippingCost { get; set; }

		[Required]
		public bool ShippingCostFixed { get; set; } = false;

		[Required]
		public bool Sales { get; set; } = false;

		[Required]
		public double TotalRange { get; set; } = 0;

		[Required]
		public double MoneyTransferFees { get; set; } = 0;

		[Required]
		[StringLength(50)]
		public string IP { get; set; } = string.Empty;

		[Required]
		public bool Native {  get; set; } = false;

		[StringLength(500)]
		public string?  Flag{ get; set; }
		 
	}
}
