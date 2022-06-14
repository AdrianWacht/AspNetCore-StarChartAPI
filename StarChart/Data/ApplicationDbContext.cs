using Microsoft.EntityFrameworkCore;
using StarChart.Models;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        private DbSet<CelestialObject> celestialObjects;

        public DbSet<CelestialObject> CelestialObjects { get => celestialObjects; set => celestialObjects = value; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
