using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{
	[Table("ClientAddresses")]
	[Index(nameof(ClientID))]
	[Index(nameof(CountryID))]
	[Index(nameof(StatusID))]
	public class ClientAddress
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
 
		[Required]
		public int ClientID { get; set; }

		[StringLength(200)]
		public string? Title { get; set; }

		[StringLength(50)]
		public string? Line1 { get; set; }

		[StringLength(50)]
		public string? Line2 { get; set; }

		[StringLength(50)]
		public string? StateProvince { get; set; }

		[StringLength(50)]
		public string? City { get; set; }

		[StringLength(50)]
		public string? PostalCode { get; set; }

		[Required]
		public int CountryID { get; set; }
		 
		public double Latitude { get; set; } = -1;

		public double Longitude { get; set; } = -1;
		 
		[Required]
		public string StatusID { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }

		[ForeignKey("ClientID")]
		public virtual Client Client { get; set; }

		[ForeignKey("CountryID")]
		public Country Country { get; set; }

		[ForeignKey("StatusID")]
		public virtual Status Status { get; set; }
		 
		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }

		[ForeignKey("DeletedUserID")]
		public virtual User DeletedUser { get; set; }


	}
}
