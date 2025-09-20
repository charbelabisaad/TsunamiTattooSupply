using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Users")]
	[Index(nameof(Username), IsUnique = true)] // Create unique index on Username
	[Index(nameof(UserTypeID))] // Non-unique index on UserTypeID
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
		[StringLength(3)]
		public string UserTypeID { get; set; }

		[ForeignKey("UserTypeID")]
		public virtual UserType UserType { get; set; }

		[Required]
		public int CreatedUserID { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[Required]
		public DateTime CreationDate { get; set; }

		[Required]
		public int EditUserID { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditedUser { get; set; }

		[Required]
		public DateTime EditDate { get; set; }

	}
}
