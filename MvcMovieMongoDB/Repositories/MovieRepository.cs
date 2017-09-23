using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MvcMovieMongoDB.Data;
using MvcMovieMongoDB.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMovieMongoDB.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;
        private readonly IElasticClient _elasticClient;

        public MovieRepository(IOptions<Settings> settings, IElasticClient elasticClient)
        {
            _context = new MovieContext(settings);
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies(string searchString)
        {
            if (searchString == null)
            {
                searchString = string.Empty;
            }

            var searchResponse = await _elasticClient.SearchAsync<Movie>(s => s
            .AllTypes()
            .From(0)
            .Size(1000)
            .Query(qry => qry
                .QueryString(qs => qs
                    .Query($"*{searchString}*"))));

            return searchResponse.Documents;
        }

        public async Task<Movie> GetMovie(string id)
        {
            var filter = Builders<Movie>.Filter.Eq("Id", id);
            return await _context.Movies
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
        }

        public async Task AddMovie(Movie item)
        {
            item.Id = Guid.NewGuid().ToString();
            await _context.Movies.InsertOneAsync(item);
            await _elasticClient.IndexAsync(item);
        }

        public async Task<DeleteResult> RemoveMovie(string id)
        {
            await _elasticClient.DeleteAsync<Movie>(id);

            //Cheesy shit cause I can't figure out how to make sure the move that was just deleted doesn't show on the index page when it reloads
            Thread.Sleep(2000);

            return await _context.Movies.DeleteOneAsync(Builders<Movie>.Filter.Eq("Id", id));
        }

        public async Task<UpdateResult> UpdateMovie(Movie item)
        {
            var filter = Builders<Movie>.Filter.Eq(s => s.Id, item.Id);
            var update = Builders<Movie>.Update
                                .Set(s => s.Title, item.Title)
                                .Set(s => s.ReleaseDate, item.ReleaseDate)
                                .Set(s => s.Genre, item.Genre)
                                .Set(s => s.Price, item.Price)
                                .Set(s => s.Rating, item.Rating);
            await _elasticClient.IndexAsync(item);

            return await _context.Movies.UpdateOneAsync(filter, update);
        }

        public async Task<ReplaceOneResult> UpdateMovie(string id, Movie item)
        {
            await _elasticClient.IndexAsync(item);

            return await _context.Movies
                                 .ReplaceOneAsync(n => n.Id.Equals(id)
                                                     , item
                                                     , new UpdateOptions { IsUpsert = true });
        }

        public async Task<DeleteResult> RemoveAllMovies()
        {
            await _elasticClient.DeleteManyAsync<Movie>(await GetAllMovies(string.Empty));

            return await _context.Movies.DeleteManyAsync(new BsonDocument());
        }
    }
}
