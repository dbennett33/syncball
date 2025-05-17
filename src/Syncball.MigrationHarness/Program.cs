using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Syncball.EF;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<SyncballContext>(options =>
            options.UseSqlServer("YourConnectionStringHere"));
    })
    .Build()
    .Run();
