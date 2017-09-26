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
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: api/customers
        public IHttpActionResult GetCustomers(string query = null)
        {

            var customersQuery = _context.Customers
                            .Include(c => c.MembershipType);

            if (!String.IsNullOrWhiteSpace(query))
            {
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));
            }

            var customers = customersQuery
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customers);
        }

        // GET: api/customers/{id}
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                NotFound();
            }

            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }

        // POST: api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var customer= Mapper.Map<CustomerDto, Customer>(customerDto);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;

            var uri = new Uri(Request.RequestUri+"/"+customer.Id);
            return Created(uri, customerDto);
        }

        // PUT: api/customers/{id}
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                BadRequest();
            }

            var currentCustomer = _context.Customers.Single(c => c.Id == id);

            if (currentCustomer == null)
            {
                NotFound();
            }

            Mapper.Map<CustomerDto, Customer>(customerDto, currentCustomer);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/customers/{id}
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var currentCustomer = _context.Customers.Single(c => c.Id == id);

            if (currentCustomer == null)
            {
                NotFound();
            }

            _context.Customers.Remove(currentCustomer);
            _context.SaveChanges();

            return Ok();
        }
    }
}
