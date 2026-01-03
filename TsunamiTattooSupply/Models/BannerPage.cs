using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("BannersPages")]
	//[Index(nameof(Code), IsUnique = true)]
	//[Index(nameof(Name), IsUnique = true)]
	[Index(nameof(StatusID))]
	public class BannerPage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(10)]
		public string Code { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string  Image { get; set; }

		[Column (TypeName = "text")]
		public string Link { get; set; }

		[Required]
		public string StatusID { get; set; }

		[Required]
		public int CreatedUserID { get; set; }

		[Required]
		public DateTime CreationDate { get; set; }

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
