using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class Book : AbstractItem
    {
        public string Author { get;  set; }//private set
        public string Summary { get;  set; }//private set

        public Book() { }

        public Book(string name,string author, CategoryName category, string publisher, float price, string summary, int? edition, DateTime printYear,string imgName, Librarian librarian)
            : base(name, category,publisher,price,edition,printYear,imgName, librarian)
        {
            Author = author;
            Summary = summary;
        }
    }
}
