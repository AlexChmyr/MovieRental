using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieRental.Models;
using MovieRental.Dtos;
using AutoMapper;
using System.Data.Entity;

namespace MovieRental.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: api/movies
        public IHttpActionResult GetMovies(string query = null)
        {
            var moviesQuery = _context.Movies
                .Include(c => c.Genre);

            if (!String.IsNullOrWhiteSpace(query))
            {
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));
            }

            var movies = moviesQuery
                .Where(c => c.NumberAvailable > 0)
                .ToList()
                .Select(Mapper.Map<Movie, MovieDto>);

            return Ok(movies);
        }

        // GET: api/movies/{id}
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                NotFound();
            }

            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }

        // POST: api/movies
        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var movie = Mapper.Map<MovieDto, Movie>(movieDto);

            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            var uri = new Uri(Request.RequestUri + "/" + movie.Id);
            return Created(uri, movieDto);
        }

        // PUT: api/movies/{id}
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var currentMovie = _context.Movies.Single(m => m.Id == id);

            if (currentMovie == null)
            {
                NotFound();
            }

            Mapper.Map<MovieDto, Movie>(movieDto, currentMovie);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/movies/{id}
        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult DeleteMovie(int id)
        {
            var currentMovie = _context.Movies.Single(c => c.Id == id);

            if (currentMovie == null)
            {
                NotFound();
            }

            _context.Movies.Remove(currentMovie);
            _context.SaveChanges();

            return Ok();
        }
    }
}
