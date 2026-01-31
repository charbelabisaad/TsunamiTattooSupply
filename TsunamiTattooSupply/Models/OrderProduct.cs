using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("OrderProducts")]
	[Index(nameof(OrderID))]
	[Index(nameof(ProductID))]
	[Index(nameof(SizeID))]
	[Index(nameof(ColorID))]
	public class OrderProduct
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int OrderID { get; set; }

		[Required]
		public int ProductID { get; set; }
		 
		[Required]
		public int ProductTypeID { get; set; }

		[Required]
		public int ProductDetailID { get; set; }
		 
		[Required]
		public int SizeID { get; set; }

		[Required]
		public int ColorID { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal Quantity { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal QuantityPending { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal QuantityDelivered { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal QuantityCancelled { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal Price { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal Discount {  get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal VATAmount { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal PriceNet { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal SubTotal { get; set; }
 
		[Column(TypeName = "text")]
		public string? CancellationReason { get; set; }
		 
		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("OrderID")]
		public virtual Order Order { get; set; }

		[ForeignKey("ProductID")]
		public virtual Product Product { get; set; }

		[ForeignKey("SizeID")]
		public virtual Size Size { get; set; }

		[ForeignKey("ColorID")]
		public virtual Color Color { get; set; }
		 
		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
