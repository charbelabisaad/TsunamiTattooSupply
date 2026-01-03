using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("TrackingOrders")]
	[Index(nameof(OrderID))]
	[Index(nameof(TrackingOrderStatusID))]
	[Index(nameof(TrackingOrderStatusID))]
	public class TrackingOrder
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int OrderID { get; set; }
		
		[Required] 
		public int TrackingOrderStatusID { get; set; }

		[Required]
		public int? CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		[ForeignKey("OrderID")]
		public virtual Order Order { get; set; }

		[ForeignKey("TrackingOrderStatusID")]
		public virtual TrackingOrderStatus TrackingOrderStatus { get; set; }

		[ForeignKey("CreatedUserID")]
		public virtual User CreatedUser { get; set; }
		 
	}
}
