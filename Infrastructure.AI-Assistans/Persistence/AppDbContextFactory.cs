
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.AI_Assistans.Persistence
{
    public class AppDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5433;Database=ai_assistant_db;Username=postgres;Password=postgres");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
    
}
