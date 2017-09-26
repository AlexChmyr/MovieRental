using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MovieRental.Models
{
    public class Rental
    {
        public int Id { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Movie Movie { get; set; }

        [Required]
        public int MovieId { get; set; }

        public DateTime DateRented { get; set; }

        public DateTime? DateReturned { get; set; }
    }
}