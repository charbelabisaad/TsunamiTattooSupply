using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ColorTypes")]
	public class ColorType
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[StringLength(3)]
		public string Code { get; set; }

		[Required]
		[StringLength(30)]
		public string Description { get; set; }

	}
}
