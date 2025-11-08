using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ProductsWishList")]
	[Index(nameof(ClientID))]
	[Index(nameof(ProductID))]
	public class ProductWishList
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ClientID { get; set; }

		[Required]
		public int ProductID { get; set; }

		public DateTime CreationDate { get; set; }

		[ForeignKey("ClientID")]
		public virtual Client Client { get; set; }

		[ForeignKey("ProductID")]
		public virtual Product Product { get; set; }
		 
	}
}
