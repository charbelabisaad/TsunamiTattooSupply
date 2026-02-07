using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Colors")]
	//[Index(nameof(Code), IsUnique = true)]
	//[Index(nameof (Name), IsUnique = true)]
	[Index(nameof(TypeID))]
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
		public int TypeID { get; set; } = 1;

		[Required]
		public bool ShowFront { get; set; } = true;

		[Required]
		public string StatusID { get; set; }

		[ForeignKey("TypeID")]
		public virtual ColorType ColorType { get; set; }	

		[ForeignKey("StatusID")]
		public virtual Status Status { get; set; }
		 

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
