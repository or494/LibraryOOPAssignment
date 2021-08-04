using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class Journal : AbstractItem
    {
        public DateTime PrintDate { get; set; } //private set

        public Journal() { }

        public Journal(string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear,DateTime printDate, string imgName, Librarian librarian)
            : base(name, category, publisher, price, edition, printYear, imgName, librarian)
        {
            PrintDate = printDate;
        }
    }
}
