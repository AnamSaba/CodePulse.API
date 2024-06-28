using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePusle.API.Data
{
	public class AuthDbContext : IdentityDbContext
	{
		public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
		{
		}


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var readerRoleId = "dd2e360f-9884-44d0-b08b-0d6e1b7c12f9";
			var writerRoleId = "eea3c617-bcfa-4a52-8b1e-9fe6470de07f";

			//Create Reader and Writer Role

			var roles = new List<IdentityRole>
			{
				new IdentityRole
				{
					Id = readerRoleId,
					ConcurrencyStamp = readerRoleId,
					Name = "Reader",
					NormalizedName = "Reader".ToUpper()
				},
				new IdentityRole
				{
					Id = writerRoleId,
					ConcurrencyStamp = writerRoleId,
					Name = "Writer",
					NormalizedName = "Writer".ToUpper()
				}
			};

			//Seed the roles

			builder.Entity<IdentityRole>().HasData(roles);

			// Create an admin user

			var adminUserId = "313f7fb6-132c-4573-b209-8f6a93dc1585";

			var admin = new IdentityUser()
			{
				Id = adminUserId,
				UserName = "admin@codepulse.com",
				Email = "admin@codepulse.com",
				NormalizedUserName = "admin@codepulse.com".ToUpper(),
				NormalizedEmail = "admin@codepulse.com".ToUpper()
			};

			admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

			builder.Entity<IdentityUser>().HasData(admin);

			// Give roles to admin

			var adminRoles = new List<IdentityUserRole<string>>()
			{
				new ()
				{
					UserId = adminUserId,
					RoleId = readerRoleId
				},
				new ()
				{
					UserId = adminUserId,
					RoleId = writerRoleId
				}
			};

			builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

		}
	}
}
