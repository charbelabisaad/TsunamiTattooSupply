
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("CoorporateOffices")]
	[Index(nameof(ContactID))]
	public class CooporateOffice
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(1000)]
		public string Location { get; set; }

		[Column(TypeName = "text")]
		public string MapLink { get; set; }

		[Required]
		[StringLength(100)]
		public string Phone { get; set; }

		[Required]
		[StringLength(2000)]
		public string Period {  get; set; }

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
