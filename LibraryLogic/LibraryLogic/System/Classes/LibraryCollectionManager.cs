using Entities.Items;
using LibraryLogic.Items;
using LibraryLogic.System.Interfaces;
using LibraryOOPAssignment;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.System.Classes
{
    public class LibraryCollectionManager : ILibraryCollectionManager
    {
        private string Name { get; set; }
        private IContextFactory ContextFactory { get; set; }

        public LibraryCollectionManager() { }

        public LibraryCollectionManager(string libraryName, IContextFactory contextFactory)
        {
            Name = libraryName;
            ContextFactory = contextFactory;
        }

        public void AddBook(string name, string author, CategoryName category, string publisher, float price, string summary,
            int? edition, DateTime printYear, string imgName)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                Librarian librarian = context.Users.FirstOrDefault(x => x.Id == LibrarySystem._userManager.GetLoggedUser().Id) as Librarian;
                if (librarian == null) throw new ArgumentException();

                Book book = new Book(name, author, category, publisher, price, summary, edition, printYear, imgName, librarian);
                context.Items.Add(book);
                context.SaveChanges();
            }
        }

        public void AddJournal(string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear,
            DateTime printDate, string imgName)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                Librarian librarian = context.Users.FirstOrDefault(x => x.Id == LibrarySystem._userManager.GetLoggedUser().Id) as Librarian;
                if (librarian == null) throw new ArgumentException();

                Journal journal = new Journal(name, category, publisher, price, edition, printYear, printDate, imgName, librarian);
                context.Items.Add(journal);
                context.SaveChanges();
            }
        }

        public void BorrowItem(AbstractItem item)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                var itemInstance = context.Items.FirstOrDefault(x => x.Id == item.Id);
                if (itemInstance == null) throw new ArgumentException();

                itemInstance.IsBorrowed = true;
                var userInstance = context.Users.FirstOrDefault(x => x.Id == LibrarySystem._userManager.GetLoggedUser().Id);
                if (userInstance == null) throw new ArgumentException();

                var borrowingHistory = new BorrowingHistory(userInstance, itemInstance);
                itemInstance.BorrowingHistories.Add(borrowingHistory);
                userInstance.BorrowingHistories.Add(borrowingHistory);
                context.SaveChanges();
            }
        }

        public void EditBook(Guid isbn, string name, CategoryName category, string publisher, float price, int? edition,
            DateTime printYear, string author, string summary, string imgName)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                var abstractItem = context.Items.FirstOrDefault(x => x.ISBN.ToString() == isbn.ToString());
                if (abstractItem == null) throw new ArgumentException();

                var item = abstractItem as Book;
                if (item == null) throw new ArgumentException();

                item.Name = name;
                item.Category = category;
                item.Publisher = publisher;
                item.Price = price;
                item.Edition = edition;
                item.PrintYear = printYear;
                item.Author = author;
                item.Summary = summary;
                item.ImgName = imgName;

                context.SaveChanges();
            }
        }

        public void EditJournal(Guid isbn, string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear, DateTime printDate, string imgName)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                var abstractItem = context.Items.FirstOrDefault(x => x.ISBN.ToString() == isbn.ToString());
                if (abstractItem == null) throw new ArgumentException();

                var item = abstractItem as Journal;
                if (item == null) throw new ArgumentException();

                item.Name = name;
                item.Category = category;
                item.Publisher = publisher;
                item.Price = price;
                item.Edition = edition;
                item.PrintYear = printYear;
                item.PrintDate = printDate;
                item.ImgName = imgName;

                context.SaveChanges();
            }
        }

        public List<string> GetAllAuthors()
        {
            List<string> ret = new List<string>();
            using(var context = ContextFactory.CreateLibraryContext())
            {
                foreach(var abstractItem in context.Items)
                {
                    Book item = abstractItem as Book;
                    if(item != null && !ret.Contains(item.Author))
                    {
                        ret.Add(item.Author);
                    }
                }
            }
            return ret;
        }

        public List<string> GetAllPublishers()
        {
            List<string> ret = new List<string>();
            using (var context = ContextFactory.CreateLibraryContext())
            {
                foreach (var item in context.Items)
                {
                    if(!ret.Contains(item.Publisher)) ret.Add(item.Publisher); 
                }
            }
            return ret;
        }

        public List<string> GetAllPublishingYears()
        {
            List<string> ret = new List<string>();
            using (var context = ContextFactory.CreateLibraryContext())
            {
                foreach (var item in context.Items)
                {
                    if (!ret.Contains(item.PrintYear.Year.ToString())) ret.Add(item.PrintYear.Year.ToString());
                }
            }
            return ret;
        }

        public List<AbstractItem> GetCurrentBorrowedItems()
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                return context.Items.Where(x => x.IsBorrowed == true).ToList();
            }
        }

        public AbstractItem GetItem(Guid isbn)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                var item = context.Items.FirstOrDefault(x => x.ISBN.ToString() == isbn.ToString());
                if (item == null) throw new ArgumentException();

                return item;
            }
        }

        public string GetLibraryName()
        {
            return Name;
        }

        public List<AbstractItem> GetUnborowedItems()
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                return context.Items.Where(x => x.IsBorrowed == false).ToList();
            }
        }

        public void RemoveItem(Guid isbn)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                var item = context.Items.FirstOrDefault(x => x.ISBN.ToString() == isbn.ToString());
                if (item == null) throw new ArgumentException();

                context.Items.Remove(item);
                context.SaveChanges();
            }
        }

        public void ReturnItem(AbstractItem item)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                var itemInstance = context.Items.FirstOrDefault(x => x.Id == item.Id);
                if (itemInstance == null) throw new ArgumentException();

                itemInstance.IsBorrowed = false;
                itemInstance.BorrowingHistories.Last().ReturningDate = DateTime.Now;

                context.SaveChanges();
            }
        }

        public List<AbstractItem> SearchFor(string name)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                return context.Items.Where(x => x.Name == name).ToList();
            }
        }

        public List<AbstractItem> GetUserBorrowedItems(Person user)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                Person person = context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (person == null) throw new ArgumentException();

                return person.BorrowingHistories.Where(x => x.ReturningDate.Year < 2000).Select(x => x.Item).ToList();
            }
        }

        public List<AbstractItem> GetUserPastBorrowedItems(Person user)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                Person person = context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (person == null) throw new ArgumentException();

                return person.BorrowingHistories.Where(x => x.ReturningDate.Year > 2000).Select(x => x.Item).ToList();
            }
        }

        public List<AbstractItem> GetUsersViewedItems()
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                Person person = context.Users.FirstOrDefault(x => x.Id == LibrarySystem._userManager.GetLoggedUser().Id);
                if (person == null) throw new ArgumentException();

                return person.ViewedItems.Select(x => x.Item).ToList();
            }
        }

        public void UserViewedItem(AbstractItem item)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                AbstractItem tmpItem = context.Items.FirstOrDefault(x => x.ISBN == item.ISBN);
                if (tmpItem == null) throw new ArgumentException();
                Person person = context.Users.FirstOrDefault(x => x.Id == LibrarySystem._userManager.GetLoggedUser().Id);
                if (person == null) throw new ArgumentException();

                var itemCustomerViewed = new ItemCustomerViewed(person, tmpItem);
                tmpItem.PersonViewed.Add(itemCustomerViewed);
                person.ViewedItems.Add(itemCustomerViewed);
                context.SaveChanges();
            }
        }
    }
}
