using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcMovieMongoDB.Repositories;
using MvcMovieMongoDB.Models;


namespace MvcMovieMongoDB.Controllers
{
    public class MovieSiteController : Controller
    {
        private readonly IMovieRepository _movieRepository;

        public MovieSiteController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // GET: MovieSite
        public async Task<IActionResult> Index(string searchString)
        {
            if (searchString == null)
            {
                searchString = string.Empty;
            }

            IEnumerable<Movie> movieList = await _movieRepository.GetAllMovies(searchString);
            return View(movieList);
        }

        // GET: MovieSite/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: MovieSite/Create
        public IActionResult Create()
        {
            ModelState.Clear();
            return View(new Movie());
        }

        // POST: MovieSite/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                await _movieRepository.AddMovie(movie);
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.GetMovie(id);
            if (movie == null)
            {
                return NotFound();
            }
            
            return View(movie);
        }

        // POST: MovieSite/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _movieRepository.UpdateMovie(movie);
                }
                catch
                {
                    if (_movieRepository.GetMovie(movie.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: MovieSite/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = new Movie();
            movie = await _movieRepository.GetMovie(id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: MovieSite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _movieRepository.RemoveMovie(id);
            return RedirectToAction("Index");
        }

        // GET: MovieSite/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // GET: MovieSite/About
        public IActionResult About()
        {
            return View();
        }
    }
}