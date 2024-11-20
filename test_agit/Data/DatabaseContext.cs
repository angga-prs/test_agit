using Microsoft.EntityFrameworkCore;
using test_agit.Models;

namespace test_agit.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ProductionPlan> tProductionPlans { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
    }
}
