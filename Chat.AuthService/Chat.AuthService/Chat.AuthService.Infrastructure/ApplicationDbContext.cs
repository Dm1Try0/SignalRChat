#nullable disable
using Chat.AuthService.Domain;
using Chat.AuthService.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chat.AuthService.Infrastructure
{
    /// <summary>
    /// Database context for current application
    /// </summary>
    public class ApplicationDbContext : DbContextBase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            return new ApplicationDbContext(optionsBuilder.Options);
        }
        public DbSet<EventItem> EventItems { get; set; }

        public DbSet<ApplicationUserProfile> Profiles { get; set; }

        public DbSet<AppPermission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseOpenIddict<Guid>();
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
           // optionsBuilder.UseNpgsql("Server=<SQL>;Database=<DatabaseName>;User ID=<UserName>;Password=<Password>");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }

#nullable restore
}