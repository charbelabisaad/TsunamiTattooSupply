using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Users")]
	[Index(nameof(Username), IsUnique = true)] // Create unique index on Username
	[Index(nameof(UserTypeID))] // Non-unique index on UserTypeID
	[Index(nameof(RoleID))] 
	[Index(nameof(StatusID))] 
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required, StringLength(50)]
		public string Username { get; set; }

		[Required, StringLength(50)]
		public string FirstName { get; set; }

		[Required, StringLength(50)]
		public string LastName { get; set; }

		[Required, Column(TypeName = "text")]
		public string Password { get; set; }

		[Required]
		public int RoleID { get; set; }

		[Required]
		[StringLength(3)]
		public string UserTypeID { get; set; }

		[Required]
		public string StatusID { get; set; }

		[Required]
		public int CreatedUserID { get; set; }
		  
		[Required]
		public DateTime CreationDate { get; set; }
		 
		public int? EditUserID { get; set; }
		 
		public DateTime? EditDate { get; set; }
		 
		public int? DeletedUserID { get; set; }

		// 🔹 Soft delete fields
		public DateTime? DeletedDate { get; set; }
 
		[ForeignKey("UserTypeID")]
		public virtual UserType UserType { get; set; }

		[ForeignKey("RoleID")]
		public virtual Role Role { get; set; }

		[ForeignKey("StatusID")]
		public virtual Status Status { get; set; }

	}
 
}
