
using Microsoft.EntityFrameworkCore;
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
			modelBuilder.Entity<User>()
				.HasOne(u => u.CreatedUser)
				.WithMany()
				.HasForeignKey(u => u.CreatedUserID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>()
				.HasOne(u => u.EditedUser)
				.WithMany()
				.HasForeignKey(u => u.EditUserID)
				.OnDelete(DeleteBehavior.Restrict);

			base.OnModelCreating(modelBuilder);
		}

	}



}
