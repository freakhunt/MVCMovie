using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MvcMovieMongoDB.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MvcMovieMongoDBTests
{
    public class MovieRepositoryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMovieRepository _svc;
        
        public MovieRepositoryMiddleware(RequestDelegate next, IMovieRepository svc)
        {
            _next = next;
            _svc = svc;
        }

        public async Task Invoke(HttpContext context, IMovieRepository svc2)
        {
            IMovieRepository svc3 = context.RequestServices.GetService(typeof(IMovieRepository)) as IMovieRepository;
            await _next(context);
        }
    }

    public static class RequestMovieRepositoryMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMovieRepository(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MovieRepositoryMiddleware>();
        }
    }
}
