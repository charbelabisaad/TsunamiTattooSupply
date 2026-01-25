using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Sizes")]
	//[Index(nameof(Description), IsUnique = true)]
	[Index(nameof(StatusID))]
	public class Size
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(100)]
		public string Description {  get; set; }

		[Required]
		public int Rank { get; set; } = 0;

		[Required]
		public bool ShowFront { get; set; } = true;

		[Required]
		public string StatusID { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

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
