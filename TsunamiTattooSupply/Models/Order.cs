using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Orders")]
	//[Index(nameof(Code), IsUnique = true)]
	//[Index(nameof(PaymentCode), IsUnique = true)]
	[Index(nameof(ClientID))]
	[Index(nameof(ClientAddressID))]
	[Index(nameof(CurrencyID))] 
	public class Order
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(50)]

		public string Code { get; set; }

		[StringLength(50)]
		public string? PaymentCode { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public int ClientID { get; set; }

		[Required]
		public int ClientAddressID { get; set; }

		[Required]
		public string? PromoCode { get; set; }

		[Required]
		public int PromoCodePercentage { get; set; }

		[Required]
		[Column(TypeName ="decimal(12,2)")]
		public decimal Tax { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal ShippingCost { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal MoneyTransferFees { get; set; }

		[Required]
		public int CurrencyID { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal VAT {  get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal VATAmount { get; set; }

		[Required]
		[Column(TypeName = "decimal(12,2)")]
		public decimal GobalDiscount { get; set; }

		[Required]
		public DateTime PlacedOn {  get; set; }
		
		public DateTime? ReceivedOn { get; set; }

		public DateTime? ConfirmedOn { get; set; }

		[Required]
		public bool IsConfirmed { get; set; } = false;

		[Column(TypeName = "decimal(12,2)")]
		public decimal? TotalFixed {  get; set; }

		[Column(TypeName = "decimal(12,2)")]
		public decimal? TotalNet { get; set; }
		  
		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ClientID")]
		public virtual Client Client { get; set; }
		 
		[ForeignKey("ClientAddressID")]
		public virtual ClientAddress ClientAddress { get; set; }

		[ForeignKey("CurrencyID")]
		public virtual Currency Currency { get; set; }
 
		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }
		 
	}
}
