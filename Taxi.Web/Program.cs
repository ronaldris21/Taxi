using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Taxi.Web.Data;

namespace Taxi.Web
{
    public class Program
    {
        private static readonly bool _isTestingDB = true;
        public static void Main(string[] args)
        {
            if (_isTestingDB)
            {
                //Execute the Bogus SeedDb for poblating the DB if Empty (Just for Testing Purposes)
                IWebHost host = CreateWebHostBuilder(args).Build();
                RunSeeding(host);
                host.Run();
            }
            else
            {
                CreateWebHostBuilder(args).Build().Run();
            }


        }

        private static void RunSeeding(IWebHost host)
        {
            IServiceScopeFactory scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                SeedDb seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait(); //Wait till data is added to DB for Testing  
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
.UseStartup<Startup>();
        }
    }
}
