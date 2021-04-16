using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SisPlanWebCoreApi.Repositories;
using SisPlanWebCoreApi.Services;
using System;
using SisPlanWebCoreApi.Entities;
namespace SisPlanWebCoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using ClienteDbContext context = new ClienteDbContext();
            ClienteEntity cliente = new ClienteEntity()
            {
                 Id = new Guid()
                ,Nome = "Mauricio Marques"
                , Idade = 50
            };

            context.Add(cliente);

            ClienteEntity cliente2 = new ClienteEntity()
            {
                Id = new Guid()
               ,
                Nome = "Barack Obama"
               ,
                Idade = 60
            };

            context.Add(cliente2);

            ClienteEntity cliente3 = new ClienteEntity()
            {
                Id = new Guid()
               ,
                Nome = "Donald Trump"
               ,
                Idade = 68
            };

            context.Add(cliente3);
            context.SaveChanges();
           
            var host = CreateHostBuilder(args).Build();

            // Initializes db.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
              //  try
              //  {
                  //  var context = services.GetRequiredService<ClienteDbContext>();
                   // var dbInitializer = services.GetRequiredService<ISeedDataService>();
                  //  dbInitializer.Initialize(context).GetAwaiter().GetResult();
              //  }
              //  catch (Exception ex)
              //  {
               //     var logger = services.GetRequiredService<ILogger<Program>>();
                //    logger.LogError(ex, "An error occurred while seeding the database.");
               // }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}
