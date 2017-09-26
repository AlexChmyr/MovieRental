using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieRental.Models;
using MovieRental.ViewModels;
using System.Data.Entity;

namespace MovieRental.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // GET: movies
        public ActionResult Index()
        {
            if (!User.IsInRole(RoleName.CanManageMovies))
            {
                return View("ReadOnlyIndex");
            }

            return View();
        }

        // GET: movies/new
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var vm = new MovieFormViewModel
            {
                Movie = new Movie(),
                Genres = genres
            };

            return View("Form", vm);
        }

        // GET: movies/edit/{id}
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var vm = new MovieFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };

            return View("Form", vm);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // POST: customer/save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MovieFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var newVM = new MovieFormViewModel
                {
                    Movie = vm.Movie,
                    Genres = _context.Genres.ToList()
                };
                return View("Form", newVM);
            }

            if (vm.Movie.Id == 0)
            {
                vm.Movie.DateAdded = DateTime.Now;
                vm.Movie.NumberAvailable = vm.Movie.NumberInStock;
                _context.Movies.Add(vm.Movie);
            }
            else
            {
                var currentMovie = _context.Movies.Single(c => c.Id == vm.Movie.Id);

                currentMovie.Name = vm.Movie.Name;
                currentMovie.ReleaseDate = vm.Movie.ReleaseDate;
                currentMovie.GenreId = vm.Movie.GenreId;
                currentMovie.NumberInStock = vm.Movie.NumberInStock;
            }


            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }

    }
}