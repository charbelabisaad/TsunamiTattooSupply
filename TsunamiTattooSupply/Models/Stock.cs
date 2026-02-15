using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Stocks")]
	[Index(nameof(ProductID))]
	[Index(nameof(SizeID))]
	[Index(nameof(ColorID))]
	[Index(nameof(ProductTypeID))]
	[Index(nameof(ProductDetailID))]
	public class Stock
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int SizeID { get; set; }

		[Required]
		public int ProductTypeID { get; set; }

		[Required]
		public int ProductDetailID { get; set; }

		[Required]
		public int ColorID { get; set; }

		[StringLength(50)]
		public string Barcode { get; set; }

		[Required]
		[Column(TypeName="decimal(12,2)")]
		public decimal Quantity { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ProductID")]
		public Product Product { get; set; }

		[ForeignKey("ProductTypeID")]
		public ProductType ProductType { get; set; }

		[ForeignKey("ProductDetailID")]
		public ProductDetail ProductDetail { get; set; }

		[ForeignKey("SizeID")]
		public Size Size { get; set; }

		[ForeignKey("ColorID")]
		public virtual Color Color { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
 
