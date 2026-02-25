using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Prices")]
	[Index(nameof(ProductID))]
	[Index(nameof(ProductTypeID))]
	[Index(nameof(ProductDetailID))]
	[Index(nameof(SizeID))]
	[Index(nameof(ColorID))] 
	[Index(nameof(StatusID))]
	public class Price
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int ProductTypeID { get; set; }

		[Required]
		public int ProductDetailID { get; set; }

		[Required]
		public int SizeID { get; set; }
		 
		public int? ColorID { get; set; } 

		[Required]
		public int CountryID { get; set; }

		[Required]
		public int CurrencyID { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal Amount {  get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal AmountNet { get; set; }

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

		[ForeignKey("SizeID")]
		public Size Size { get; set; }

		[ForeignKey("ColorID")]
		public virtual Color Color { get; set; }

		[ForeignKey("CountryID")]
		public Country Country { get; set; }

		[ForeignKey("CurrencyID")]
		public Currency Currency { get; set; }

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
