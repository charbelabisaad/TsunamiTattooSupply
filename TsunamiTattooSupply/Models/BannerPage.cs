using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("BannersPages")]
	[Index(nameof(PageLocationID))]
	[Index(nameof(CategoryID))]
	[Index(nameof(SubCategoryID))]
	[Index(nameof(ProductID))]
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
		 
		[StringLength(3)]
		public string AppType { get; set; }

		[Required]
		public int PageLocationID { get; set; }
		 
		[Required]
		[Column(TypeName = "text")]
		public string  Image { get; set; }

		[Column (TypeName = "text")]
		public string? Link { get; set; }

		public int? CategoryID { get; set; } 

		public int? SubCategoryID { get; set; }

		public int? ProductID { get; set; }

		[Required]
		public bool HasPeriod { get; set; } = false;

		[Column(TypeName = "date")]
		public DateTime? StartDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime? EndDate { get; set; }

		public bool Present { get; set; } = false;
		 
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

		[ForeignKey("PageLocationID")]
		public virtual PageLocation PageLocation { get; set; }

		[ForeignKey("CategoryID")]
		public virtual Category? Category { get; set; }

		[ForeignKey("SubCategoryID")]
		public virtual SubCategory? SubCategory { get; set; }

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
