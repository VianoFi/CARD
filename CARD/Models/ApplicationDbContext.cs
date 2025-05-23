using Microsoft.EntityFrameworkCore;

namespace CardCollectionApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<PokemonCard> PokemonCards { get; set; }
        public DbSet<YuGiOhCard> YuGiOhCards { get; set; }
        public DbSet<MagicCard> MagicCards { get; set; }
    }
}
