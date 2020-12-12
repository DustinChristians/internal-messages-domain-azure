using Internal.Messages.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Internal.Messages.Repository.Data
{
    public class InternalMessagesContext : DbContext
    {
        public InternalMessagesContext(DbContextOptions<InternalMessagesContext> options)
            : base(options)
        {
        }

        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        // When the database is created, EF creates tables that have names the same as the DbSet property names.
        // Property names for collections are typically plural (Students rather than Student) so we create them
        // here without plural names.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageEntity>().ToTable("Message");
            modelBuilder.Entity<SettingEntity>().ToTable("Setting");
            modelBuilder.Entity<UserEntity>().ToTable("User");
        }
    }
}
