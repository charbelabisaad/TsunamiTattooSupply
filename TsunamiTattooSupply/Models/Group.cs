using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Groups")]
	[Index(nameof(TypeID))]
	[Index(nameof(StatusID))]
	public class Group
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Column(TypeName = "text")]
		public string? Summary { get; set; }

		[Column(TypeName = "text")]
		public string? Image { get; set; }

		[Required]
		public bool ShowHome { get; set; } = true;

		[Required]
		public int Rank { get; set; } = 0;

		[Required]
		[StringLength(3)]
		public string TypeID { get; set; }   

		[Required]
		public string StatusID { get; set; }


		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("TypeID")]
		public virtual GroupType GroupType { get; set;}

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
