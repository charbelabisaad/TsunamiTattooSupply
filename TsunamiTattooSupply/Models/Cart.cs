using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Carts")]
	[Index(nameof(ClientID))]
	[Index(nameof(ProductID))]
	[Index(nameof(SizeID))]
	[Index(nameof(ColorID))]
	public class Cart
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ClientID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int SizeID { get; set; }

		[Required]
		public int ColorID { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal Quantity { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ClientID")]
		public Client Client { get; set; }

		[ForeignKey("ProductID")]
		public Product Product { get; set; }

		[ForeignKey("SizeID")]
		public Size Size { get; set; }

		[ForeignKey("ColorID")]
		public Color Color { get; set; }
		 
		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }
 
	}
}
