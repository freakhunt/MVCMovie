using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MongoDB.Driver;
using Moq;
using MvcMovieMongoDB.Controllers;
using MvcMovieMongoDB.Models;
using MvcMovieMongoDB.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static MongoDB.Driver.DeleteResult;

namespace MvcMovieMongoDBTests.Unit_Tests.Controllers
{
    public class MovieSiteControllerShould
    {
        private MovieSiteController _movieSiteController;
        private Mock<IMovieRepository> _movieRepository;
        private MovieTestHelpers _movieTestHelpers;

        public MovieSiteControllerShould()
        {
            _movieRepository = new Mock<IMovieRepository>();
            _movieSiteController = new MovieSiteController(_movieRepository.Object);
            _movieTestHelpers = new MovieTestHelpers();
        }

        [Fact]
        public async Task VerifyIndexReturnsAllMovies()
        {
            List<Movie> expectedMovieList = _movieTestHelpers.GetAListOfTwoMovieObjects();

            _movieRepository.Setup(x => x.GetAllMovies(string.Empty)).ReturnsAsync(expectedMovieList);

            ViewResult indexResult = await _movieSiteController.Index(string.Empty) as ViewResult;
            List<Movie> moviesViewModelResult = indexResult.ViewData.Model as List<Movie>;

            Assert.NotNull(indexResult);
            Assert.Equal(expectedMovieList.Count, moviesViewModelResult.Count);
            Assert.Equal(expectedMovieList[0].Id, moviesViewModelResult[0].Id);
            Assert.Equal(expectedMovieList[1].Genre, moviesViewModelResult[1].Genre);
        }

        [Fact]
        public async Task VerifyDetailsReturnsAMovie()
        {
            Movie expectedMovie = _movieTestHelpers.GetAMovieObject();

            _movieRepository.Setup(x => x.GetMovie(expectedMovie.Id)).ReturnsAsync(expectedMovie);

            ViewResult detailsresult = await _movieSiteController.Details(expectedMovie.Id) as ViewResult;
            Movie movieViewModelResult = detailsresult.ViewData.Model as Movie;

            Assert.NotNull(detailsresult);
            Assert.NotNull(movieViewModelResult);
            Assert.Equal(expectedMovie.Id, movieViewModelResult.Id);
            Assert.Equal(expectedMovie.Genre, movieViewModelResult.Genre);
        }

        [Fact]
        public async Task VerifyCreateFailsWithModelValidationError()
        {
            Movie expectedMovie = _movieTestHelpers.GetAMovieObject();

            _movieRepository.Setup(x => x.AddMovie(expectedMovie)).Returns(Task.FromResult<Movie>(expectedMovie));

            //set model error
            _movieSiteController.ModelState.AddModelError("test", "test");

            ViewResult createViewModelresult = await _movieSiteController.Create(expectedMovie) as ViewResult;

            //clear model error
            _movieSiteController.ModelState.Clear();

            Movie movieResult = createViewModelresult.ViewData.Model as Movie;

            Assert.NotNull(createViewModelresult);
            Assert.NotNull(movieResult);
            Assert.Equal(expectedMovie.Id, movieResult.Id);
            Assert.Equal(expectedMovie.Genre, movieResult.Genre);
        }

        [Fact]
        public async Task VerifyCreateInsertsAMovie()
        {
            Movie expectedMovie = _movieTestHelpers.GetAMovieObject();
            Movie addedMovie = null;

            _movieRepository.Setup(x => x.AddMovie(expectedMovie)).Returns(Task.FromResult<Movie>(expectedMovie)).Callback<Movie>(o => addedMovie = o);

            //clear any possible model error
            _movieSiteController.ModelState.Clear();

            var createViewModelResult = await _movieSiteController.Create(expectedMovie);
            
            Assert.NotNull(createViewModelResult);
            Assert.NotNull(addedMovie);
            Assert.Equal(expectedMovie.Id, addedMovie.Id);
            Assert.Equal(expectedMovie.Genre, addedMovie.Genre);
        }

        [Fact]
        public async Task VerifyEditFailsWithModelValidationError()
        {
            Movie expectedMovie = _movieTestHelpers.GetAMovieObject();
            UpdateResult updateResult = new UpdateResult.Acknowledged(1, 1, expectedMovie.Id);

            _movieRepository.Setup(x => x.UpdateMovie(expectedMovie)).ReturnsAsync(updateResult);

            //set model error
            _movieSiteController.ModelState.AddModelError("test", "test");

            var updateControllerResult = await _movieSiteController.Edit(expectedMovie.Id, expectedMovie) as ViewResult;

            //clear model error
            _movieSiteController.ModelState.Clear();

            var movieViewModelResult = updateControllerResult.ViewData.Model as Movie;
            
            Assert.NotNull(updateControllerResult);
            Assert.NotNull(movieViewModelResult);
            Assert.Equal(expectedMovie.Id, movieViewModelResult.Id);
            Assert.Equal(expectedMovie.Genre, movieViewModelResult.Genre);
        }

        [Fact]
        public async Task VerifyEditUpdatesAMovie()
        {
            Movie expectedMovie = _movieTestHelpers.GetAMovieObject();
            Movie updatedMovie = null;
            UpdateResult updateResult = new UpdateResult.Acknowledged(1, 1, expectedMovie.Id);

            _movieRepository.Setup(x => x.UpdateMovie(expectedMovie)).ReturnsAsync(updateResult).Callback<Movie>(o => updatedMovie = o);

            //clear any possible model error
            _movieSiteController.ModelState.Clear();

            var createresult = await _movieSiteController.Edit(expectedMovie.Id, expectedMovie);

            Assert.NotNull(createresult);
            Assert.NotNull(updatedMovie);
            Assert.Equal(expectedMovie.Id, updatedMovie.Id);
            Assert.Equal(expectedMovie.Genre, updatedMovie.Genre);
        }
    }
}