using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreBuilder.Data
{
    // EF Core design-time için DbContext oluþturan fabrika.
    // Migrations oluþtururken bu yapý kullanýlarak iliþkisel (SQLite) saðlayýcý saðlanýr.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Migrations/test amaçlý SQLite kullanýyoruz (dosya bazlý).
            // Ýsterseniz burayý SQL Server baðlantýnýza göre deðiþtirin:
            // optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CoreBuilder;Trusted_Connection=True;");
            optionsBuilder.UseSqlite("Data Source=CoreBuilder.migrations.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}