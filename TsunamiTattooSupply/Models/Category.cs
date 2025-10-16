using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Categories")]
	[Index(nameof(Description),IsUnique =  true)]
	public class Category
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]
		public string Description { get; set; }

		[StringLength(50)]
		public string? BannerImage { get; set; }

		[StringLength(50)]
		public string? Image {  get; set; }

		[StringLength(50)]
		public string? AD_Image1 { get; set; }

		[StringLength(50)]
		public string? AD_Image2 { get; set; }

		[StringLength(50)]
		public string? AD_Image3 { get; set; }

		[Column(TypeName ="text")]
		public string? AD_Details { get; set; }

		[StringLength(50)]
		public string? MobileImage { get; set; }

		[Required]
		public int Rank { get; set; } = 0;

		[Required]
		public bool Active { get; set; } = true;

		public int CreatedUserID { get; set; }

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
