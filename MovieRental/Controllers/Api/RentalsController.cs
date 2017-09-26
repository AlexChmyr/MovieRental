using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieRental.Dtos;
using MovieRental.Models;

namespace MovieRental.Controllers.Api
{
    public class RentalsController : ApiController
    {
        private ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult CreateRentals(RentalDto rentalDto)
        {
            if (rentalDto.MovieIds.Count == 0)
            {
                return BadRequest("No movies selected");
            }

            var customer = _context.Customers.SingleOrDefault(c => c.Id == rentalDto.CustomerId);

            if (customer == null)
            {
                return BadRequest("Invalid customer ID");
            }

            var movies = _context.Movies.Where(m => rentalDto.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                {
                    return BadRequest(movie.Name+ ": movie is not available");
                }

                movie.NumberAvailable--;

                var rental = new Rental
                {
                    CustomerId = customer.Id,
                    MovieId = movie.Id,
                    DateRented = DateTime.Now
                };

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteRentals(RentalDto rentalDto)
        {
            if (rentalDto.MovieIds.Count == 0)
            {
                return BadRequest("No movies selected");
            }

            var customer = _context.Customers.SingleOrDefault(c => c.Id == rentalDto.CustomerId);

            if (customer == null)
            {
                return BadRequest("Invalid customer ID");
            }

            var movies = _context.Movies.Where(m => rentalDto.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                try
                {
                    var rental = _context.Rentals.FirstOrDefault(r => r.CustomerId == rentalDto.CustomerId && r.MovieId == movie.Id && r.DateReturned == null);

                    if (rental == null)
                    {
                        return BadRequest("Customer " + customer.Name + " did not rent this video: " + movie.Name);
                    }

                    rental.DateReturned = DateTime.Now;
                    movie.NumberAvailable++;
                }
                catch (Exception e)
                {
                    throw e;
                }

               
            }

            _context.SaveChanges();

            return Ok();
        }

    }
}
