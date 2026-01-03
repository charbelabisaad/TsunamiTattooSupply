using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Products")]
	//[Index(nameof(Code), IsUnique = true)]
	//[Index(nameof(Name), IsUnique = true)]
	[Index(nameof(UnitID))]
	[Index(nameof(GroupID))]
	[Index(nameof(StatusID))]
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(100)]
		public string Code { get; set; }

		[Required]
		[StringLength(200)]
		public string Name { get; set; }

		[Column(TypeName="text")]
		public string? Description { get; set; }

		[Required]
		public int UnitID { get; set; }

		[Required]
		public int GroupID { get; set; }
		 
		[Required]
		public bool VAT {  get; set; }

		[Required] 
		public bool Feature { get; set; }

		[Required]
		public bool NewArrival { get; set; }

		[Column(TypeName = "date")]
		public DateTime? NewArrivalDateExpiryDate { get; set; }

		[Required]
		public bool Warranty { get; set; }

		[Required]
		public int WarrantyMonths {  get; set; }

		[Column(TypeName = "text")]
		public string? VideoUrl {  get; set; }

		[Required]
		public int Rank { get; set; } = 0;

		[Required]
		public string StatusID { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("UnitID")]
		public virtual Unit Unit { get; set; }
		 
		[ForeignKey("GroupID")]
		public virtual Group Group { get; set; }

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
