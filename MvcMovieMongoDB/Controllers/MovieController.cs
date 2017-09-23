using Microsoft.AspNetCore.Mvc;
using MvcMovieMongoDB.Models;
using MvcMovieMongoDB.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcMovieMongoDB.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET /GetAllMovies
        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("GetAllMovies")]
        public Task<IEnumerable<Movie>> Get()
        {
            return GetMoviesInternal();
        }

        private async Task<IEnumerable<Movie>> GetMoviesInternal()
        {
            string searchString = string.Empty;
            return await _movieRepository.GetAllMovies(searchString);
        }

        // GET /{id}
        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet("{id}")]
        public Task<Movie> Get(string id)
        {
            return GetMovieByIdInternal(id);
        }

        private async Task<Movie> GetMovieByIdInternal(string id)
        {
            return await _movieRepository.GetMovie(id) ?? new Movie();
        }

        // POST /Add
        [HttpPost("Add")]
        public void Post([FromBody]Movie value)
        {
            _movieRepository.AddMovie(new Movie()
            {
                Title = value.Title,
                ReleaseDate = value.ReleaseDate,
                Genre = value.Genre,
                Price = value.Price,
                Rating = value.Rating
            });
        }

        // PUT /Update/{id}
        [HttpPut("Update/{id}")]
        public void Put(string id, [FromBody]Movie value)
        {
            _movieRepository.UpdateMovie(id, value);
        }

        // DELETE api/movies/{id}
        [HttpDelete("Delete/{id}")]
        public void Delete(string id)
        {
            _movieRepository.RemoveMovie(id);
        }
    }
}
