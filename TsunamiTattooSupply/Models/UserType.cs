using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("UserTypes")]
	public class UserType
	{
		[Key]
		[StringLength(3, MinimumLength=3)]
		public string ID { get; set; }

		[StringLength(50)]
		[Required]
		public string Description { get; set; }

	}
}
