using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.DTO
{
	public class CurrencyDto
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[StringLength(3)]
		public string Code { get; set; }

		[Required]
		[StringLength(100)]
		public string Description { get; set; }

		[Required]
		[StringLength(5)]
		public string Symbol { get; set; }

	}
}
