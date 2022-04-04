using BookLibrary.ActionFilters;
using BookLibrary.Models.Entities;
using BookLibrary.ExtentionMethods;
using BookLibrary.Models;
using BookLibrary.Models.Home;
using BookLibrary.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookLibraryDbContext context;

        public HomeController(BookLibraryDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var model = new IndexVM()
            {
                BookCollection = this.context.Books.ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginVM model)
        {

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            User loggedUser = this.context.Users.
                Where(m => m.Username == model.Username &&
                              m.Password == model.Password)
                .FirstOrDefault();

            if (loggedUser == null)
            {
                this.ModelState.AddModelError("authError", "Invalid password or username!");
                return View(model);
            }

            HttpContext.Session.SetObject("loggedUser", loggedUser);



            Subscription subscription = this.context.Subscriptions.
                Where(x => x.UserId == loggedUser.UserId).
                FirstOrDefault();

            if (subscription != null)
            {
                if (subscription.ExpiryDate.CompareTo(DateTime.Now) < 0)
                {
                    this.context.Subscriptions.Remove(subscription);
                    this.context.SaveChanges();
                    subscription = null;
                }
                HttpContext.Session.SetObject("subscription", subscription);
            }


            return RedirectToAction("Index", "Home");

        }

        [AuthenticationFilterAttrubutes]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");
            HttpContext.Session.Remove("subscription");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!this.ModelState.IsValid)
                return View(model);


            User newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password,
                TypeOfUser = "normal"
            };

            User UserWithThisUsername = this.context.Users.
                Where(u => u.Username == model.Username).
                FirstOrDefault();

            if (UserWithThisUsername != null)
            {
                this.ModelState.AddModelError("usernameError", "❗The username is taken. Try another.");
                return View(model);
            }

            this.context.Add(newUser);
            this.context.SaveChanges();

            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Subscribe(string button)
        {
            Subscription subscription = new Subscription();
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");


            switch (button)
            {
                case "oneMonth":

                    subscription.Name = "One Month";
                    subscription.Price = 5.90;
                    subscription.ExpiryDate = DateTime.Now.AddMonths(1);
                    subscription.Duration = subscription.ExpiryDate.Date.Subtract(DateTime.Now.Date).TotalDays;
                    subscription.UserId = loggedUser.UserId;
                    break;

                case "sixMonths":

                    subscription.Name = "Six Months";
                    subscription.Price = 29.90;
                    subscription.ExpiryDate = DateTime.Now.AddMonths(6);
                    subscription.Duration = subscription.ExpiryDate.Date.Subtract(DateTime.Now.Date).TotalDays;
                    subscription.UserId = loggedUser.UserId;
                    break;

                case "twelveMonths":

                    subscription.Name = "Twelve Months";
                    subscription.Price = 53.90;
                    subscription.ExpiryDate = DateTime.Now.AddMonths(12);
                    subscription.Duration = subscription.ExpiryDate.Date.Subtract(DateTime.Now.Date).TotalDays;
                    subscription.UserId = loggedUser.UserId;
                    break;
                default:
                    break;

            }

           this.context.Subscriptions.Add(subscription);
           this.context.SaveChanges();


            subscription = this.context.Subscriptions.
                Where(m => m.UserId == loggedUser.UserId)
                .FirstOrDefault();

            HttpContext.Session.SetObject("subscription", subscription);

            return RedirectToAction("Index", "Home");
        }
    }
}
