using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TsunamiTattooSupply.Models
{
	[Table("SubCategories")]
	//[Index(nameof(Description), IsUnique = true)]
	[Index(nameof(CategoryID))]
	[Index(nameof(StatusID))]   // Non-unique index on StatusID
	public class SubCategory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(500)]
		public string Description { get; set; }

		[Required]
		[StringLength(50)]
		public string? SpecsLabel { get; set; }	

		[StringLength(500)]
		public string? BannerImage { get; set; }

		[StringLength(500)]
		public string? WebImage { get; set; }
  
		[StringLength(500)]
		public string? MobileImage { get; set; }

		[Required]
		public int Rank { get; set; } = 0;

		[Required]
		public int CategoryID { get; set; }

		[Required]
		public string StatusID { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("CategoryID")]
		public virtual Category Category { get; set; }

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
