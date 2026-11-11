using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace btl_webnc_G08.Data;
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> {
 public ApplicationDbContext CreateDbContext(string[] args) => new(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NeighborRide_G08_Dev;Trusted_Connection=True;TrustServerCertificate=True").Options);
}
