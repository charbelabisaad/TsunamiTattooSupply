using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ContactDetails")]
	[Index(nameof(ContactID))]
	public class ContactDetail
	{

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(100)]
		public string Phone { get; set; }

		[Required]
		[StringLength(300)]
		public string Email { get; set; }

		[Required]
		public int ContactID { get; set; }

		[ForeignKey("ContactID")]
		public virtual ContactType Contact { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
