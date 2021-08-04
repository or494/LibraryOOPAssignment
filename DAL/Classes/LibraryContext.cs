using LibraryOOPAssignment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LibraryContext : DbContext
    {
        public DbSet<AbstractItem> Items { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Person> Users { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public string DbPath { get; private set; }

        public LibraryContext() : base() 
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}blogging.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>();
            builder.Entity<Journal>();

            builder.Entity<Librarian>();
            builder.Entity<Customer>();

            builder.Entity<Librarian>().HasData(new Librarian { FirstName = "", LastName = "", Age = 22, Id = "123456789", Passward = "123456789"});
        }
    }
}
