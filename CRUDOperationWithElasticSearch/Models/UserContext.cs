using Microsoft.EntityFrameworkCore;
using MyUser.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyUser.Models
{
    public class UserContext : DbContext
    {
        public static string ConnectionString = "server=localhost;integrated security=True; database=UserDB;TrustServerCertificate=true;";

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Backpack> Backpacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();

            var users = new List<User>()
                {
                    new User(){ FirstName = "Jahonger", LastName = "Ahmedov", Username = "User1", Password = "User11" },
                    new User(){ FirstName = "Jake", LastName = "Esh" , Username = "User2", Password = "User22" },
                    new User(){ FirstName = "Rasul", LastName = "Azimov" , Username = "User3", Password = "User33" },
                };

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.FullName).HasComputedColumnSql($"[{nameof(User.FirstName)}] + ' ' + [{nameof(User.LastName)}]", true);

                entity.HasMany(c => c.Backpacks)
                .WithOne(e => e.User).HasForeignKey(e=>e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.HasData(users);
            });

            var firstUser = users.First();
            var backpacks = new List<Backpack>()
                {
                    new Backpack(){ Name = "First", UserId = firstUser.Id },
                    new Backpack(){ Name = "Second", UserId = firstUser.Id },
                };
            modelBuilder.Entity<Backpack>(entity =>
            {
                entity.ToTable("Backpacks");
                entity.HasData(backpacks);
            });
        }
    }
}
