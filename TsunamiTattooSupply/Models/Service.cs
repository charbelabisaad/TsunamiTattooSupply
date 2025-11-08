using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Services")]
	public class Service
	{
		[Key]
		public string ID { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string Text { get; set; }

		[Required]
		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }
		 
	}
}
