using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("POSStocks")]
	//[Index(nameof(Barcode), IsUnique = true)]
	public class POSStock
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Barcode { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal Quantity { get; set; }
		 
	}
}
