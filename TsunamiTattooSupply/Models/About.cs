using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Abouts")]
	public class About
	{
		[Key]
		[StringLength(10)]
		public string ID { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string ShortText1 { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string ShortText2 { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string LongText {  get; set; }

		[Column(TypeName = "text")]
		public string? Image1 {  get; set; }

		[Column(TypeName = "text")]
		public string? Image2 { get; set; }
		 
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
