using Microsoft.EntityFrameworkCore;
using SisPlanWebCoreApi.Entities;

namespace SisPlanWebCoreApi.Repositories
{
    public class ClienteDbContext : DbContext
    {
        public ClienteDbContext()
        {
        }

        public ClienteDbContext(DbContextOptions<ClienteDbContext> options)
           : base(options)
        {

        }

        public DbSet<ClienteEntity> ClienteItens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source =(localdb)\\MSSQLLocalDB;Initial Catalog=Clientes");

        }

    }
}
