using SisPlanWebCoreApi.Entities;
using SisPlanWebCoreApi.Repositories;
using System;
using System.Threading.Tasks;

namespace SisPlanWebCoreApi.Services
{
    public class SeedDataService : ISeedDataService
    {
        public async Task Initialize(ClienteDbContext context)
        {
            context.ClienteItens.Add(new ClienteEntity() { Nome = "Mauricio Marques", Idade = 50 });
            context.ClienteItens.Add(new ClienteEntity() { Nome = "Barack Obama", Idade = 65 });
            context.ClienteItens.Add(new ClienteEntity() { Nome = "Donald Trumo", Idade = 68 });

            await context.SaveChangesAsync();
        }
    }
}
