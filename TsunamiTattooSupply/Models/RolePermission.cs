using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Index(nameof(RoleID))]
	[Index(nameof(PermissionID))]
	[Index(nameof(CreatedUserID))]
	public class RolePermission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int RoleID { get; set; }

		[Required]
		[StringLength(20)]
		public int PermissionID { get; set; }

		[ForeignKey("RoleID")]
		public virtual Role Role { get; set; }

		[ForeignKey("PermissionID")]
		public virtual Permission Permission { get; set; }

		[Required]
		public int CreatedUserID { get; set; }

		[Required]
		public DateTime CreationDate { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

	}
}
