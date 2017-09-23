using GenFu;
using System.Net.Http.Headers;
using MvcMovieMongoDB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MvcMovieMongoDBTests
{
    public class MovieTestHelpers : IMovieTestHelpers
    {
        public Movie GetAMovieObject()
        {
            return new Movie()
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Title 1",
                Genre = "Genre 1",
                Price = 1.23M,
                Rating = "R",
                ReleaseDate = DateTime.Now
            };
        }

        public List<Movie> GetAListOfTwoMovieObjects()
        {
            return new List<Movie>()
            {
                new Movie() { Id = Guid.NewGuid().ToString(), Title = "Title 1", Genre = "Genre 1", Price = 1.23M, Rating = "R", ReleaseDate = DateTime.Now },
                new Movie() { Id = Guid.NewGuid().ToString(), Title = "Title 2", Genre = "Genre 2", Price = 1.23M, Rating = "R", ReleaseDate = DateTime.Now }
            };
        }

        public Movie GetAGeneratedMovieObject()
        {
            A.Configure<Movie>()
                .Fill(x => x.Id, () => Guid.NewGuid().ToString())
                .Fill(x => x.Price).WithRandom(new decimal?[] { 1, 90 } )
                .Fill(x => x.Rating).WithRandom(GetListOfMovieRatings())
                .Fill(x => x.Genre).WithRandom(GetListOfMovieGenres())
                .Fill(x => x.ReleaseDate).Maggie.GetGenericFillerForType(typeof(DateTime?));

            var movie = A.New<Movie>();

            return movie;
        }

        public List<Movie> GetAListOfGeneratedMovieObjects(int numberOfObjects)
        {
            A.Configure<Movie>()
                .Fill(x => x.Id, () => Guid.NewGuid().ToString())
                .Fill(x => x.Price).WithRandom(new decimal?[] { 1, 90 })
                .Fill(x => x.Rating).WithRandom(GetListOfMovieRatings())
                .Fill(x => x.Genre).WithRandom(GetListOfMovieGenres())
                .Fill(x => x.ReleaseDate).Maggie.GetGenericFillerForType(typeof(DateTime?));

            var movie = A.ListOf<Movie>(numberOfObjects);

            return movie;
        }

        public string[] GetListOfMovieGenres()
        {
            Movie movie = GetAMovieObject();
            var genres = movie.Genres;
            List<string> genresToReturn = new List<string>();

            foreach(var genre in genres)
            {
                genresToReturn.Add(genre.Value);
            }

            return genresToReturn.ToArray();
        }

        public string[] GetListOfMovieRatings()
        {
            Movie movie = GetAMovieObject();
            var ratings = movie.Ratings;
            List<string> ratingsToReturn = new List<string>();

            foreach (var rating in ratings)
            {
                ratingsToReturn.Add(rating.Value);
            }

            return ratingsToReturn.ToArray();
        }

        public ByteArrayContent AddObjectToBodyOfRequestContent(Movie movie)
        {
            var myContent = JsonConvert.SerializeObject(movie);

            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}
