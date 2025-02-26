using static loginAPP.DbInitializer.DBInitialize;
using Microsoft.Extensions.DependencyInjection;

namespace loginAPP.DbInitializer
{
    public static class RunInitializer
    {
        
            public static void RunDatabaseInitializer(this IApplicationBuilder app)
            {
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetService<IDBInitialize>();

                    dbInitializer.Initialize();
                    dbInitializer.SeedData();
                }
            }
        
    }
}
