using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace maps.Models
{
    public class context : DbContext
    {
        public static string connectionString = "Server=127.0.0.1;Port=5432;Database=K;User Id=postgres;Password=Vq2R8FJ;";
        public DbSet<admin> admins { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<feedback> feedbacks { get; set; }
        public DbSet<route> routes { get; set; }
        public DbSet<place> places { get; set; }
        public context() : base()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<feedback>()
                 .HasOne(b => b.user)
                 .WithMany(a => a.feedbacks);

            modelBuilder.Entity<feedback>()
                 .HasOne(b => b.route)
                 .WithMany(a => a.feedbacks);

            modelBuilder.Entity<place>()
               .HasMany(c => c.routes)
               .WithMany(s => s.places)
               .UsingEntity(j => j.ToTable("routeplace"));
        }
        internal object FromSqlRaw(string v, string w)
        {
            throw new NotImplementedException();
        }
    }
}
