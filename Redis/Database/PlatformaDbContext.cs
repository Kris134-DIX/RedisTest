using Microsoft.EntityFrameworkCore;
using Redis.Database.Models;


namespace Redis.Database
{
    public class PlatformaDbContext : DbContext
    {
        public DbSet<SubFundUnitPriceModel> SubFundUnitPrices { get; set; }
        public PlatformaDbContext(DbContextOptions<PlatformaDbContext> options) : base(options) { }

        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        //=> options.UseSqlServer("Data Source=tsql2\\tsql;Initial Catalog=Itm.Xelion.Platforma_TEST;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
    }
}
