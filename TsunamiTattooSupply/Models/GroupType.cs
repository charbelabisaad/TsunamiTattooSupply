using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("GroupTypes")]
	public class GroupType
	{
		[Key]
		[StringLength(3)]
		public string ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Description { get; set; }

	}
}
