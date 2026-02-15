using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace TsunamiTattooSupply.Models
{
	[Table("PromoCodes")]
	[Index(nameof(ClientID))]
	[Index(nameof(StatusID))]
	public class PromoCode
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public string Code { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public int Percentage {  get; set; }

		[Required]
		[StringLength(3)]
		public string Level { get; set; }

		[Required]
		public DateTime ExpiryDate { get; set; }
		 
		[Required]
		public int ClientID { get; set; }

		[Required]
		public string StatusID { get; set; }

		[ForeignKey("ClientID")]
		public Client Client { get; set; }	

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
