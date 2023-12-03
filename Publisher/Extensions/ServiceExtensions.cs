using Contracts.Configuration;
using Messaging;
using Publisher.ActionFilters;
using Publisher.Core;
using Publisher.HostedServices;
using Publisher.Interface;
using Repository;

namespace Publisher.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

    public static void ConfigurePublisherApp(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ValidationFilterAttribute>();

        builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqSection"));
        builder.Services.Configure<CachingConfiguration>(builder.Configuration.GetSection("CachingConfiguration"));

        builder.Services.AddHostedService<RabbitMqReceiverServiceHost>();

        builder.Services.AddSingleton<IDataProcessor, DataProcessor>();
        builder.Services.AddSingleton<ISchedulerProvider, SchedulerProvider>();
        builder.Services.AddSingleton<ICacheProvider, CacheProvider>();
        builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
        builder.Services.AddSingleton<ITimeHelper, TimeHelper>();

        builder.Services.AddAutoMapper(typeof(Program));

        // see PublisherDbContextFactory.cs implementing IDesignTimeDbContextFactory
        builder.Services.AddDbContext<PublisherDbContext>(
            //options => options.UseNpgsql(builder.Configuration.GetConnectionString("postgreSqlConnection"),
            //    b => b.MigrationsAssembly("Repository"))
        );

        builder.Services.AddMemoryCache();
    }
}