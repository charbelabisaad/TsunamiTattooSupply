using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("Currencies")]
	//[Index(nameof(Code), IsUnique = true)]
	//[Index(nameof(Description), IsUnique = true)]
	[Index(nameof(StatusID))]   // Non-unique index on StatusID
	[Index(nameof(CountryID))]
	public class Currency
	{
		[Key]
		public  int ID { get; set; }

		[Required]
		[StringLength(3)]
		public string Code { get; set; }

		[Required]
		[StringLength (100)]
		public string Description { get; set; }

		[Required]
		[StringLength (5)]
		public string Symbol { get; set; }

		[Required]
		[StringLength (4)]
		public string Priority { get; set; }

		public int? CountryID { get; set; }
		 
		[Required]
		public string StatusID { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("CountryID")]
		public virtual Country Country { get; set; }

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
