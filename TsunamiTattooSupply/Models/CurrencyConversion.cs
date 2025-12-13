using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("CurrenciesConversion")]
	[Index(nameof(CurrencyIDFrom))]
	[Index(nameof(CurrencyIDTo))]

	public class CurrencyConversion
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required] 
		public int CurrencyIDFrom { get; set; }

		[Required] 
		public int CurrencyIDTo { get; set; }

		[Required]
		[StringLength(3)]
		public string Operator { get; set; }

		[Required]
		[Column(TypeName = "Decimal(12,2)")]
		public decimal Rate { get; set; }

		[ForeignKey("CurrencyIDFrom")]
		public virtual Currency CurrencyFrom { get; set; }

		[ForeignKey("CurrencyIDTo")]
		public virtual Currency CurrencyTo { get; set; }
		 
		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

	}
}
