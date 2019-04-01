using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class NotificallDbContext : DbContext
    {
        public DbSet<Source> Sources { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Call> Calls { get; set; }

        public NotificallDbContext(DbContextOptions<NotificallDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(x => x.Text)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Source)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
