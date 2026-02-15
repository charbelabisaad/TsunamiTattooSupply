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
	[Index(nameof(CountryID))]
	[Index(nameof(CurrencyID))]
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
		public int ProductTypeID { get; set; }

		[Required]
		public int ProductDetailID { get; set; }

		[Required]
		public int SizeID { get; set; }

		[Required]
		public int ColorID { get; set; }
		 
		public int? CountryID { get; set; }

		public int? CurrencyID { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal Quantity { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal Discount { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal Price { get; set; }
		 
		[Column(TypeName = "decimal(12,2)")]
		public decimal PriceNet { get; set; }
		 
		[Column(TypeName = "decimal(12,2)")]
		public decimal SubTotal { get; set; }
		  
		public DateTime CreationDate { get; set; }
		  
		public DateTime? EditDate { get; set; }
		  
		[ForeignKey("ClientID")]
		public Client Client { get; set; }

		[ForeignKey("ProductID")]
		public Product Product { get; set; }

		[ForeignKey("ProductTypeID")]
		public ProductType ProductType { get; set; }

		[ForeignKey("ProductDetailID")]
		public ProductDetail ProductDetail { get; set; }

		[ForeignKey("SizeID")]
		public Size Size { get; set; }

		[ForeignKey("ColorID")]
		public Color Color { get; set; }
 
		[ForeignKey("CountryID")]
		public Country Country { get; set; }

		[ForeignKey("CurrencyID")]
		public Currency Currency { get; set; }
		  
	}
}
