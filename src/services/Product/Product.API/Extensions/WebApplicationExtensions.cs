using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Polly;
using Polly.Retry;

namespace Product.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MigrateDbContext<TContext>(this WebApplication webApp, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using IServiceScope scope = webApp.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        ILogger<TContext> logger = services.GetRequiredService<ILogger<TContext>>();
        TContext context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

            int retries = 5;

            RetryPolicy retry = Policy.Handle<SqlException>()
                .WaitAndRetry(
                    retryCount: retries,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(
                            exception,
                            "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                            nameof(TContext),
                            exception.GetType().Name,
                            exception.Message,
                            retry,
                            retries);
                    });

            retry.Execute(() => InvokeSeeder(seeder, context, services));

            logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while migrating the database used on context {DbContextName}",
                typeof(TContext).Name);
        }

        return webApp;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
