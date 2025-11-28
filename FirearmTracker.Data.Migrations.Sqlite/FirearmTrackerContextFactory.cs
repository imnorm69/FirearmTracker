using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FirearmTracker.Data.Migrations.Sqlite
{
    public class FirearmTrackerContextFactory : IDesignTimeDbContextFactory<FirearmTrackerContext>
    {
        public FirearmTrackerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FirearmTrackerContext>();

            optionsBuilder.UseSqlite("Data Source=dummy.db",
                x => x.MigrationsAssembly("FirearmTracker.Data.Migrations.Sqlite"));

            return new FirearmTrackerContext(optionsBuilder.Options);
        }
    }
}