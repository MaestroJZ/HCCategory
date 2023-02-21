using Microsoft.EntityFrameworkCore;

namespace HCCategory.Models
{
    public class CContext : DbContext
    {
        public CContext(DbContextOptions<CContext> options) : base(options) { }
            
        public DbSet<Category> Categories { get; set; }    
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
