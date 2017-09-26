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
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: customer
        public ActionResult Index()
        {
            return View();
        }

        // GET: customer/new
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var vm = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };

            return View("Form", vm);
        }

        // GET: customer/details
        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: customer/edit/{id}
        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var vm = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            return View("Form", vm);
        }

        // POST: customer/save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CustomerFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var newVM = new CustomerFormViewModel
                {
                    Customer = vm.Customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("Form", newVM);
            }

            if (vm.Customer.Id == 0)
            {
                _context.Customers.Add(vm.Customer);
            }
            else
            {
                var currentCustomer = _context.Customers.Single(c => c.Id == vm.Customer.Id);

                currentCustomer.Name = vm.Customer.Name;
                currentCustomer.Birthdate = vm.Customer.Birthdate;
                currentCustomer.MembershipTypeId = vm.Customer.MembershipTypeId;
                currentCustomer.IsSubscribedToNotification = vm.Customer.IsSubscribedToNotification;
                currentCustomer.Birthdate = vm.Customer.Birthdate;
                currentCustomer.MembershipTypeId = vm.Customer.MembershipTypeId;
            }


            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}