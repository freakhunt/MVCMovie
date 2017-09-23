
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using MvcMovieMongoDB.Controllers;
using MvcMovieMongoDB.Models;
using MvcMovieMongoDB.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcMovieMongoDBTests.Integrated_Tests.Controllers
{
    public class MovieControllerShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private MovieTestHelpers _movieTestHelpers;

        public MovieControllerShould()
        {
            _server = new TestServer(new WebHostBuilder()
            .UseStartup<StartupTests>());

            _client = _server.CreateClient();

            _movieTestHelpers = new MovieTestHelpers();
        }

        [Fact]
        public async Task ReturnAllMovies()
        {
            var response = await _client.GetAsync("/GetAllMovies");
            response.EnsureSuccessStatusCode();
            var str = response.Content.ReadAsStringAsync();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ReturnAMovie()
        {
            var response = await _client.GetAsync("/f82e2743-3c44-45b6-aef6-c795fbd3029e");
            response.EnsureSuccessStatusCode();
            var str = response.Content.ReadAsStringAsync();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SaveAMovie()
        {
            var movie = _movieTestHelpers.GetAMovieObject();
            ByteArrayContent byteContent = _movieTestHelpers.AddObjectToBodyOfRequestContent(movie);

            var response = await _client.PostAsync("/Add", byteContent);
            response.EnsureSuccessStatusCode();
            var str = response.Content.ReadAsStringAsync();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task UpdateAMovie()
        {
            var movie = _movieTestHelpers.GetAMovieObject();
            ByteArrayContent byteContent = _movieTestHelpers.AddObjectToBodyOfRequestContent(movie);

            var response = await _client.PutAsync($"/Update/{movie.Id}", byteContent);
            response.EnsureSuccessStatusCode();
            var str = response.Content.ReadAsStringAsync();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SaveThenDeleteAMovie()
        {
            var movie = _movieTestHelpers.GetAGeneratedMovieObject();
            ByteArrayContent byteContent = _movieTestHelpers.AddObjectToBodyOfRequestContent(movie);

            var saveResponse = await _client.PostAsync("/Add", byteContent);
            saveResponse.EnsureSuccessStatusCode();

            //Delete the movie that was just created
            var response = await _client.DeleteAsync($"/Delete/{movie.Id}");
            response.EnsureSuccessStatusCode();
            var str = response.Content.ReadAsStringAsync();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task SaveAList100OfMovies()
        {
            var movies = _movieTestHelpers.GetAListOfGeneratedMovieObjects(100);

            foreach (Movie movie in movies)
            {
                ByteArrayContent byteContent = _movieTestHelpers.AddObjectToBodyOfRequestContent(movie);

                var response = await _client.PostAsync("/Add", byteContent);
                response.EnsureSuccessStatusCode();
                var str = response.Content.ReadAsStringAsync();
                Assert.NotNull(response);
            }
        }
    }
}
