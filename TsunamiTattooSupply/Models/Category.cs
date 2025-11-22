using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Categories")]
	//[Index(nameof(Description),IsUnique =  true)]
	[Index(nameof(StatusID))]   // Non-unique index on StatusID
	public class Category
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(500)]
		public string Description { get; set; }

		[StringLength(500)]
		public string? BannerImage { get; set; }

		[StringLength(500)]
		public string? WebImage {  get; set; }

		[StringLength(500)]
		public string? AD_Image1 { get; set; }

		[StringLength(500)]
		public string? AD_Image2 { get; set; }

		[StringLength(500)]
		public string? AD_Image3 { get; set; }

		[Column(TypeName ="text")]
		public string? Details { get; set; }

		[StringLength(500)]
		public string? MobileImage { get; set; }

		[Required]
		public int Rank { get; set; } = 0;

		[Required]
		public string StatusID { get; set; } 

		public int CreatedUserID { get; set; }

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
