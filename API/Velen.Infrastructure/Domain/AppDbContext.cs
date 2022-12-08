using Microsoft.EntityFrameworkCore;
using Velen.Infrastructure.Processing.InternalCommands;
using Velen.Infrastructure.Processing.Outbox;

namespace Velen.Infrastructure.Domain
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
          
        }
        public DbSet<InternalCommand> InternalCommands { get; set; }
        
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
