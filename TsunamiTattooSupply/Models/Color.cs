using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Colors")]
	[Index(nameof(Code), IsUnique = true)]
	[Index(nameof (Name), IsUnique = true)]
	[Index(nameof(StatusID))]
	public class Color
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		 
		[StringLength(6)]
		public string? Code { get; set; }

		[Required]
		[StringLength(300)]
		public string Name { get; set; }
		 
		[Required]
		public string StatusID { get; set; }

		[ForeignKey("StatusID")]
		public virtual Status Status { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
