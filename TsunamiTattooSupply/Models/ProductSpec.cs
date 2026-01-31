using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ProductsSpecs")]
	[Index(nameof(ProductID))]
	[Index(nameof(SpecID))]	
	public class ProductSpec
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ProductID { get; set; }

		[Required]
		public int SpecID { get; set; }

		[Required]
		public int CreatedUserID { get; set; } 

		[Required]
		public int CreatedDate { get; set; }

		public int? DeleteUserID {  get; set; }
		
		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ProductID")]
		public virtual Product Product { get; set; }

		[ForeignKey("SpecID")]
		public virtual Spec Spec { get; set; }
		 
		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }

	}
}
