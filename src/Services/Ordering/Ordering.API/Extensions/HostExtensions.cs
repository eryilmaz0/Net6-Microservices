using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions;

public static class HostExtensions
{
     public static IServiceCollection MigrateDatabase<TContext>(this IServiceCollection serviceCollection, 
                                                Action<TContext, IServiceProvider> seeder) where TContext : DbContext

    {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                
                var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
                var context = serviceProvider.GetRequiredService<TContext>();
    
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
    
                    //InvokeSeeder(seeder, context, serviceProvider);
                    var retry = Policy.Handle<SqlException>()
                    .WaitAndRetry(
                         retryCount: 50,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3),
                         onRetry: (exception, retryCount, context) =>
                         {
                             logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                         });

                    retry.Execute(() => InvokeSeeder(seeder, context, serviceProvider));


                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);                   
                }
                
                return serviceCollection;
            }
    
     
            private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, 
                                                        TContext context, 
                                                        IServiceProvider services)
                                                        where TContext : DbContext
            {
                context.Database.Migrate();
                seeder(context, services);
            }
}