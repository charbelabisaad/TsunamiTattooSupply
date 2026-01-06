using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("SocialMedias")]
	[Index(nameof(Code), IsUnique = true)]
	[Index(nameof(Description), IsUnique = true)]
	public class SocialMedia
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(10)]
		public string Code { get; set; }

		[Required]
		[StringLength(50)]
		public string Description { get; set; }

		[Required]
		[StringLength(100)]
		public string Icon { get; set; }

		[Required]
		public int Rank { get; set; }

	}
}
