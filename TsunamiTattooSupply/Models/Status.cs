using System.ComponentModel.DataAnnotations;

namespace TsunamiTattooSupply.Models
{
	public class Status
	{
		[Key]
		[StringLength(1, MinimumLength = 1)]
		public string ID { get; set; }

		[Required]
		[MaxLength(50)]
		public string Description { get; set; }

		[Required]
		[StringLength(6, MinimumLength = 6)]
		public string Color { get; set; }

	}
}
