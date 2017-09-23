using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MvcMovieMongoDB.Models;

namespace MvcMovieMongoDB.Repositories
{
    public interface IMovieRepository
    {
        Task AddMovie(Movie item);
        Task<IEnumerable<Movie>> GetAllMovies(string searchString);
        Task<Movie> GetMovie(string id);
        Task<DeleteResult> RemoveAllMovies();
        Task<DeleteResult> RemoveMovie(string id);
        Task<UpdateResult> UpdateMovie(Movie item);
        Task<ReplaceOneResult> UpdateMovie(string id, Movie item);
    }
}