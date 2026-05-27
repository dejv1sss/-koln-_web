using Microsoft.EntityFrameworkCore;
using skolni_web.Models;

namespace skolni_web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tahak> Tahaky { get; set; }
        public DbSet<OdpadnutaHodina> OdpadnuteHodiny { get; set; }
    }
}