using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovieMongoDB.Data
{
    public class MovieContext
    {
        private readonly IMongoDatabase _database = null;

        public MovieContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Models.Movie> Movies
        {
            get
            {
                return _database.GetCollection<Models.Movie>("Movie");
            }
        }
    }
}
