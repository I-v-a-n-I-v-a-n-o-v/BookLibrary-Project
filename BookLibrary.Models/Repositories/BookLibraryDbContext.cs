using BookLibrary.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Models.Repositories
{
    public class BookLibraryDbContext : DbContext
    {
        public BookLibraryDbContext()
        {
            this.Users = this.Set<User>();
            this.Books = this.Set<Book>();
            this.Genres = this.Set<Genre>();
            this.RentedBooks = this.Set<RentedBook>();
            this.Summaries = this.Set<Summary>();
            this.Subscriptions = this.Set<Subscription>();
            this.Likes = this.Set<Like>();
            this.Comments = this.Set<Comment>();
            this.Images = this.Set<Image>();
        }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             .UseSqlServer(@"Server = DESKTOP-4VGPC6R\SQLEXPRESS;Database=BookLibraryDB;Trusted_Connection=True;")
             .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserId = 1,
                    Username = "admin",
                    Password = "admin",
                    FirstName = "Test",
                    LastName = "Test"
                });

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique(); 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<RentedBook> RentedBooks { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
