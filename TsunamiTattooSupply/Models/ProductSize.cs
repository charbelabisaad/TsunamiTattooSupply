using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ProductsSizes")]
	[Index(nameof(ProductID))]
	[Index(nameof(SizeID))]
	[Index(nameof(StatusID))]
	public class ProductSize
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int SizeID { get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal Sale {  get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal Raise { get; set; }

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
