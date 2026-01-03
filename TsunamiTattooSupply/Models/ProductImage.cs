using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ProductsImages")]
	[Index(nameof(ProductID))]
	[Index(nameof(ColorID))]
	[Index(nameof(StatusID))]
	public class ProductImage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int ID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int  ColorID { get; set; }
		 
		[Column(TypeName = "text")]
		public string? SmallImage { get; set; }

		[Column(TypeName = "text")]
		public string? OriginalImage { get; set; }

		[Required]
		public bool IsInitial { get; set; }
		 
		[Required]
		public string StatusID { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ProductID")]
		public Product Product { get; set; }

		[ForeignKey("ColorID")]
		public Color Color { get; set; }

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
