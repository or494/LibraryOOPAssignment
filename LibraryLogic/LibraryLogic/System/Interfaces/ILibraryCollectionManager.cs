using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.System.Interfaces
{
    public interface ILibraryCollectionManager
    {
        void AddBook(string name, string author, CategoryName category, string publisher, float price, string summary, int? edition, DateTime printYear, string imgName);

        void EditBook(Guid isbn, string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear,
           string author, string summary, string imgName);

        void AddJournal(string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear, DateTime printDate, string imgName);

        void EditJournal(Guid isbn, string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear, DateTime printDate, string imgName);

        void RemoveItem(Guid isbn);

        void BorrowItem(AbstractItem item);

        void ReturnItem(AbstractItem item);

        AbstractItem GetItem(Guid isbn);

        List<AbstractItem> SearchFor(string name);

        List<string> GetAllAuthors();

        List<string> GetAllPublishers();

        List<string> GetAllPublishingYears();

        List<AbstractItem> GetUnborowedItems();

        List<AbstractItem> GetCurrentBorrowedItems();

        string GetLibraryName();

        List<AbstractItem> GetUserBorrowedItems(Person user);

        List<AbstractItem> GetUserPastBorrowedItems(Person user);

        List<AbstractItem> GetUsersViewedItems();

        void UserViewedItem(AbstractItem item);
    }
}
