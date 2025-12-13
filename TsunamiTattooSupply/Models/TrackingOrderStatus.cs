using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TsunamiTattooSupply.Models
{ 
	public class TrackingOrderStatus
	{
		 
		public int ID { get; set; }
		 
		public string Code { get; set; }
		 
		public string Description { get; set; }
		 
	}
}
