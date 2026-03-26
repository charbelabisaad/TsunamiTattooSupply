using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("BannersMobiles")]
	[Index(nameof(CategoryID))]
	[Index(nameof(SubCategoryID))]
	[Index(nameof(GroupID))]
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
		[StringLength(50)]
		public string Description { get; set; } = string.Empty;
 
		public int? CategoryID { get; set; }
		 
		public int? SubCategoryID { get; set; }
 
		public int? GroupID { get; set; }

		public int? ProductID { get; set; }

		public bool ShopNow { get; set; } = false;

		[Required]
		public int Rank { get; set; } = 0;

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
		public virtual Category? Category { get; set; }

		[ForeignKey("SubCategoryID")]
		public virtual SubCategory? SubCategory { get; set; }

		[ForeignKey("GroupID")]
		public virtual Group? Group { get; set; }

		[ForeignKey("ProductID")]
		public virtual Product? Product { get; set; }

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
