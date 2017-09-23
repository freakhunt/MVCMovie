using System.Collections.Generic;
using MvcMovieMongoDB.Models;
using System.Net.Http;

namespace MvcMovieMongoDBTests
{
    public interface IMovieTestHelpers
    {
        List<Movie> GetAListOfTwoMovieObjects();
        Movie GetAMovieObject();
        ByteArrayContent AddObjectToBodyOfRequestContent(Movie movie);
        Movie GetAGeneratedMovieObject();
        List<Movie> GetAListOfGeneratedMovieObjects(int numberOfObjects);
    }
}