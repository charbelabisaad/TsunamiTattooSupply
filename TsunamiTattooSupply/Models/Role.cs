using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Roles")]
	//[Index(nameof(Description), IsUnique = true)]
	[Index(nameof(StatusID))]
	[Index(nameof(CreatedUserID))]
	[Index(nameof(EditUserID))]
	[Index(nameof(DeletedUserID))]
	public class Role
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Description { get; set; }

		[Required]
		public string StatusID { get; set; }

		[Required]
		public bool IsAdmin {  get; set; } = false;
 
		public int? CreatedUserID { get; set; }
		 
		public DateTime? CreationDate { get; set; }
		 
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
