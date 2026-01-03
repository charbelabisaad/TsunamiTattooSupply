using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("BannersMobiles")]
	[Index(nameof(CategoryID))]
	[Index(nameof(SubCategoryID))]
	[Index(nameof(ProductID))]
	[Index(nameof(StatusID))]
	public class BannerMobile
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[Column(TypeName = "text")]
		public string Image { get; set; }

		[Required]
		public int CategoryID { get; set; }

		[Required]
		public int SubCategoryID { get; set; }

		[Required]
		public int ProductID { get; set; }

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

		[ForeignKey("CategoryID")]
		public virtual Category Category { get; set; }

		[ForeignKey("SubCategoryID")]
		public virtual SubCategory SubCategory { get; set; }

		[ForeignKey("ProductID")]
		public virtual Product Product { get; set; }

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
