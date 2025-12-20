using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TsunamiTattooSupply.Models;

namespace TsunamiTattooSupply.DTO
{
	public class ProductDto
	{

		 
		public int ID { get; set; }
		 
		public string? ImagePath { get; set; }
		public string? Image {  get; set; }

		public string Code { get; set; }
 
		public string Name { get; set; }
		 
		public string? Description { get; set; }
 
		public int UnitID { get; set; }

		public string UnitShortDescription { get; set; }

		public string UnitLongDescription { get; set; }
		 
		public string GroupTypeID { get; set; }

		public int GroupID { get; set; }

		public string GroupDescription { get; set; }
 
		public bool VAT { get; set; }
 
		public bool Feature { get; set; }
		 
		public bool NewArrival { get; set; }
 
		public DateTime? NewArrivalDateExpiryDate { get; set; }
 
		public bool Warranty { get; set; }
 
		public int WarrantyMonths { get; set; }
 
		public string VideoUrl { get; set; }
 
		public int Rank { get; set; } = 0;
 
		public string StatusID { get; set; }

		public string StatusDescription {  get; set; }

		public string StatusColor { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		public int? DeletedUserID { get; set; }

		public DateTime? DeletedDate { get; set; }
  
	}
}
