using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using BookLibrary.Models.Entities;
using BookLibrary.ExtentionMethods;
using BookLibrary.Models.Books;
using BookLibrary.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace BookLibrary.Controllers
{
    public class BookController : Controller
    {

        private readonly BookLibraryDbContext context;

        public BookController(BookLibraryDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexVM()
            {
                BookCollection = this.context.Books.ToList(),
                Summary = this.context.Summaries.ToList()
            };

            return this.View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {

            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            Book currentB = this.context.Books.
                Where(l => l.Id == id).
                 FirstOrDefault();


            if (currentB == null)
                return RedirectToAction("Index", "Book");


            var item = new DetailsVM()
            {
                Heading = currentB.Heading,
                Pages = currentB.Pages,
                Quantity = currentB.Quantity,
                Description = currentB.Summary.Content != null ? currentB.Summary.Content : "",
                Genre = currentB.Genre.Name,
                BookId = currentB.Id,
                ImageUrl = currentB.Image.Url != null ? currentB.Image.Url : "",
                CountOfLikes = this.context.Likes.Where(l => l.BookId == currentB.Id).ToList().Count,
                Comments = this.context.Comments.Where(l => l.BookId == currentB.Id).ToList()
            };

            if (loggedUser != null)
            {
                List<RentedBook> CurrentRentedBooks = this.context.RentedBooks.Where(l => l.BookId == id &&
                                                                              l.UserId == loggedUser.UserId).ToList();
                List<RentedBook> AllRentedBooks = this.context.RentedBooks.Where(l => l.UserId == loggedUser.UserId).ToList();

                item.CountOfAllOrderedBooks = AllRentedBooks.Count;

                if (CurrentRentedBooks != null)
                {
                    item.CountOfOrderedBooks = CurrentRentedBooks.Count;
                }
            }


            return this.View(item);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            var current = this.context.Books.Where(l => l.Id == id).FirstOrDefault();

            if (current == null)
                return RedirectToAction("Index", "Book");


            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))

                return RedirectToAction("Index", "Book");


            var model = new EditVM()
            {
                GenreCollection = context.Genres.ToList(),
                Summary = current.Summary,
                Author = current.Author,
                Heading = current.Heading,
                ReleaseDate = current.ReleaseDate,
                Pages = current.Pages,
                Quantity = current.Quantity,
                Image = current.Image
            };

            return this.View(model);

        }

        [HttpPost]
        public IActionResult Edit(EditVM model)
        {
            Book item = new Book()
            {
                Id = model.Id,
                Summary = model.Summary,
                Genre = model.Genre,
                Heading = model.Heading,
                Author = model.Author,
                Pages = model.Pages,
                Quantity = model.Quantity,
                ReleaseDate = model.ReleaseDate,
                GenreId = model.GenreID,
                Image = model.Image
            };

            this.context.Books.Update(item);
            this.context.SaveChanges();
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public IActionResult Create()
        {

            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                return RedirectToAction("Index", "Book");
            }

            var model = new CreateVM()
            {
                GenreCollection = context.Genres.ToList()
            };

            return this.View(model);

        }


        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                ModelState.AddModelError("summaryError", "You don't have permission to add books!");
                return View(model);
            }


            var item = new Book()
            {
                Summary = model.Summary,
                Genre = model.Genre,
                Heading = model.Heading,
                Author = model.Author,
                Pages = model.Pages,
                Quantity = model.Quantity,
                ReleaseDate = model.ReleaseDate,
                GenreId = model.GenreID,
                Image = model.Image
            };

            this.context.Books.Add(item);
            this.context.SaveChanges();
            return RedirectToAction("Index", "Book");


        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");

            Book current = this.context.Books.
                Where(l => l.Id == id).
                FirstOrDefault();

            if (current == null)
            {
                return RedirectToAction("Index", "Book");
            }

            if (loggedUser == null || (loggedUser.TypeOfUser != "admin"
                                       && loggedUser.TypeOfUser != "moderator"))
            {
                return RedirectToAction("Index", "Book");
            }

            var model = new DeleteVM()
            {
                Id = current.Id,
                UserType = loggedUser.TypeOfUser

            };

            return this.View(model);

        }

        [HttpPost]
        public IActionResult Delete(DeleteVM model)
        {
            var item = this.context.Books.Find(model.Id);
            this.context.Books.Remove(item);
            this.context.SaveChanges();

            return RedirectToAction("Index", "Book");
        }


        [HttpPost]
        public IActionResult Like(int BookId)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            if (loggedUser != null)
            {
                List<Like> userLikes =
                    this.context.Likes.Where(l => l.UserId == loggedUser.UserId && l.BookId == BookId).ToList();

                Subscription currentSubscription =
                    this.context.Subscriptions.Where(u => u.UserId == loggedUser.UserId).FirstOrDefault();

                if (currentSubscription != null && (currentSubscription.Name == "Six Months" ||
                                                    currentSubscription.Name == "Twelve Months"))
                {
                    if (userLikes.Count == 0)
                    {
                        Like item = new Like()
                        {
                            BookId = BookId,
                            UserId = loggedUser.UserId
                        };

                        this.context.Likes.Add(item);
                        this.context.SaveChanges();

                    }
                }
            }

            string link = "/Book/Details?id=" + BookId;
            return Redirect(link);

        }


        [HttpPost]
        public IActionResult Comment(DetailsVM model)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");


            if (loggedUser != null && model.ContentComment != null)
            {
                Subscription currentSubscription =
                    this.context.Subscriptions.
                        Where(u => u.UserId == loggedUser.UserId).
                        FirstOrDefault();

                if (currentSubscription != null && (currentSubscription.Name == "Six Months" || 
                                                    currentSubscription.Name == "Twelve Months"))
                {
                    Comment comment = new Comment()
                    {
                      UserId = loggedUser.UserId,
                      BookId = model.BookId,
                      Content = model.ContentComment,
                    };

                    this.context.Comments.Add(comment);
                    this.context.SaveChanges();
                }
            }
            string link = "/Book/Details?id=" + model.BookId;
            return Redirect(link);


        }

        public IActionResult DeleteComment(DetailsVM model)
        {
            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            Comment com = this.context.Comments.
                Where(x => x.BookId == model.BookId &&
                                 x.UserId == loggedUser.UserId)
                .FirstOrDefault();

            this.context.Comments.Remove(com);
            this.context.SaveChanges();

            string link = "/Book/Details?id=" + model.BookId;
            return Redirect(link);
        }

        [HttpPost]
        public IActionResult Order(DetailsVM model)
        {


            User loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            Book currentBook = this.context.Books.
                Where(l => l.Id == model.BookId).
                FirstOrDefault();

            List<RentedBook> RentedBooks = this.context.RentedBooks
                .Where(l => l.UserId == loggedUser.UserId).
                ToList();

            model.CountOfOrderedBooks = this.context.RentedBooks.
                                              Where(l => l.BookId == model.BookId 
                                                         && l.UserId == loggedUser.UserId).ToList()
                                                       .Count;

            if (loggedUser != null)
            {
                Subscription currentSubscription =
                    this.context.Subscriptions.
                        Where(u => u.UserId == loggedUser.UserId).
                        FirstOrDefault();

                if (currentSubscription != null)
                {
                    RentedBook rentedBook = new RentedBook();
                    rentedBook.DateOfRent = DateTime.Now;
                    rentedBook.UserId = loggedUser.UserId;
                    rentedBook.BookId = currentBook.Id;

                    if ((currentSubscription.Name == "Six Months" && RentedBooks.Count < 10) ||
                        (currentSubscription.Name == "Twelve Months" && RentedBooks.Count < 20) ||
                        (currentSubscription.Name == "One Month" && RentedBooks.Count < 5))
                    {
                        if (currentBook.Quantity > 0)
                        {
                            currentBook.Quantity--;
                            this.context.Books.Update(currentBook);
                            this.context.RentedBooks.Add(rentedBook);
                            this.context.SaveChanges();
                        }

                    }

                    model.CountOfAllOrderedBooks = RentedBooks.Count;

                }
            }

            string link = "/Book/Details?id=" + model.BookId;
            return Redirect(link);


        }

    }
}
