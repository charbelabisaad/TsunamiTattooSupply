using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{ 
 
	public class Contact
	{
		[Key]
		[Required]
		[StringLength(10)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string ID { get; set; }
 
		[Required]
		[Column(TypeName="text")]
		public string Location { get; set; }

		[Required]
		[Column(TypeName="text")]
		public string LocationMapLink { get; set; }

		[Required]
		[StringLength(100)]
		public string Phone { get; set; }

		[Required]
		[StringLength(300)]
		public string Email { get; set; }
		 
		[Required]
		public int CreatedUserID { get; set; }

		[Required]
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
