using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Abouts")]
	public class About
	{
		[Key]
		public string ID { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string ShortText { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string LongText {  get; set; }

		[Column(TypeName = "text")]
		public string? Image {  get; set; }

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
