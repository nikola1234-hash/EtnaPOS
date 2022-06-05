using EtnaPOS.DAL.Models;
using System.Data.Entity;

namespace EtnaPOS.DAL.DataAccess
{
    public class EtnaDbContext : DbContext
    {
        public EtnaDbContext() : base("EtnaDb")
        {
            
        }
        public DbSet<User>Users { get;set; }
        public DbSet<Artikal> Artikli { get; set; }
        public DbSet<KategorijaArtikla> Kategorije { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Document> Documents { get; set; }

    }
}
