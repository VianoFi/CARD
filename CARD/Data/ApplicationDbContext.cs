using Microsoft.EntityFrameworkCore;
using CARD.Models;

namespace CARD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<PokemonCard> PokemonCards { get; set; }
        public DbSet<YuGiOhCard> YuGiOhCards { get; set; }
        public DbSet<MagicCard> MagicCards { get; set; }
    }
}

