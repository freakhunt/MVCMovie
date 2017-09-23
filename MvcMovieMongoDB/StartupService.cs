using System;
using Microsoft.Extensions.DependencyInjection;
using MvcMovieMongoDB.Repositories;
using Nest;
using Microsoft.Extensions.Configuration;

namespace MvcMovieMongoDB
{
    public class StartupService
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("movies");

            services.Configure<Settings>(options =>
            {
                options.ConnectionString = configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<IMovieRepository, MovieRepository>();

            services.AddSingleton<IElasticClient>(new ElasticClient(settings));
        }
    }
}
