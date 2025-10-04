
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using TsunamiTattooSupply.Models; 
namespace TsunamiTattooSupply.Data
{
	public class TsunamiDbContext :DbContext
	{
		public TsunamiDbContext(DbContextOptions<TsunamiDbContext> options) : base(options) { }

		public DbSet<UserType> UserTypes { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Status> Statuses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{ 
			base.OnModelCreating(modelBuilder);
			 
		}

	}



}
