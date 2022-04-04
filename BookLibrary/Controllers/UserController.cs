using BookLibrary.Models.Entities;
using BookLibrary.ExtentionMethods;
using BookLibrary.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using BookLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookLibrary.Controllers
{
    public class UserController : Controller
    {
        private readonly BookLibraryDbContext context;

        public UserController(BookLibraryDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(IndexVM model)
        {

            model.allUsers = context.Users.ToList();

            return View(model);
        }

        public IActionResult Create()
        {
            CreateVM model = new CreateVM();
            
            model.Types = new List<string> { "moderator", "admin", "normal" };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {

            User user = new User()
            {
                Username = model.Username,
                Password = model.Password,
                FirstName = model.Firstname,
                LastName = model.LastName,
                TypeOfUser = model.TypeOfUser
            };
            this.context.Users.Add(user);
            this.context.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            User current = context.Users.
                Where(l => l.UserId == id).
                FirstOrDefault();

            EditVM model = new EditVM()
            {
                Username = current.Username,
                Password = current.Password,
                TypeOfUser = current.TypeOfUser,
                Firstname = current.FirstName,
                LastName = current.LastName,
                Types = new List<string> { "moderator", "admin", "normal" }

            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {

            User user = new User()
            {
                UserId = model.Id,
                Username = model.Username,
                Password = model.Password,
                TypeOfUser = model.TypeOfUser,
                FirstName = model.Firstname,
                LastName = model.LastName
            };

            this.context.Users.Update(user);
            this.context.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
           
           
            User current = context.Users.Where(user => user.UserId == id).FirstOrDefault();

            DeleteVm model = new DeleteVm();
            model.Id = current.UserId;


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(DeleteVm model)
        {
            User user = context.Users.
                 Find(model.Id);

            this.context.Users.Remove(user);
            this.context.SaveChanges();
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult UserInfo()
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            //User current = context.Users.Where(user => user.Id == id).FirstOrDefault();
            UserInfoVM model = new UserInfoVM();

            model.Username = loggedUser.Username;
            model.Password = loggedUser.Password;
            model.Firstname = loggedUser.FirstName;
            model.LastName = loggedUser.LastName;
            return View(model);
        }

        [HttpPost]
        public IActionResult UserInfo(UserInfoVM model)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            loggedUser.Username = model.Username;
            loggedUser.Password = model.Password;
            loggedUser.FirstName = model.Firstname;
            loggedUser.LastName = model.LastName;

            this.context.Users.Update(loggedUser);
            this.context.SaveChanges();

            HttpContext.Session.Remove("loggedUser");
            HttpContext.Session.Remove("subscription");
            return RedirectToAction("Login", "Home");
        }
    }
}