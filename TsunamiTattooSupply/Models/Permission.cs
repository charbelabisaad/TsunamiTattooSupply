using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Permissions")]
	//[Index(nameof(Code), IsUnique = true)]
	//[Index(nameof(Description), IsUnique = true)]
	public class Permission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(20)]
		public string Code { get; set; }

		[Required]
		[StringLength(500)]
		public string Description { get; set; }

	}
}
