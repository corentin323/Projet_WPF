using Microsoft.EntityFrameworkCore;

namespace PRojet_NPF.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Login> Logins { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Spell> Spells { get; set; }

        private static string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=ExerciceHero;Trusted_Connection=True;TrustServerCertificate=True;";

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>().ToTable("Login");
            modelBuilder.Entity<Hero>().ToTable("Hero");
            modelBuilder.Entity<Spell>().ToTable("Spell");

            modelBuilder.Entity<Hero>()
                .HasMany(h => h.Spells)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "HeroSpell",
                    j => j.HasOne<Spell>().WithMany().HasForeignKey("SpellID"),
                    j => j.HasOne<Hero>().WithMany().HasForeignKey("HeroID")
                );
        }
    }
}