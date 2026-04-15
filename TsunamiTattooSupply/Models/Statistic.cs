using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TsunamiTattooSupply.Models
{
	[Index(nameof(StatusID))]
	public class Statistic
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[StringLength(10)]
		public string Code { get; set; }

		[Required]
		[StringLength(10)]
		public string Name { get; set; }

		[Required]
		public bool IsCalculated { get; set; }

		[Required]
		public int Number { get; set; } = 0;

		[Required]
		public string StatusID { get; set; }

		public int CreatedUserID { get; set; }

		public DateTime CreationDate { get; set; }

		public int? EditUserID { get; set; }

		public DateTime? EditDate { get; set; }

		[ForeignKey("EditUserID")]
		public virtual User EditUser { get; set; }
	}
}
