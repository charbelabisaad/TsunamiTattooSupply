using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ProductsBestSeller")]
	[Index(nameof(ProductID))]
	[Index(nameof(SizeID))]
	[Index(nameof(ColorID))]
	public class ProductBestSeller
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int SizeID { get; set; }

		[Required]
		public int ColorID { get; set; }

		[Required]
		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		[ForeignKey("ProductID")]
		public virtual Product Product { get; set; }

		[ForeignKey("SizeID")]
		public virtual Size Size { get; set; }

		[ForeignKey("ColorID")]
		public virtual Color Color { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

	}
}
