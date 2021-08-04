
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class Librarian : Person
    {
        public virtual ICollection<Report> ReportsCreated { get; set; }

        public virtual ICollection<AbstractItem> ItemsCreated { get; set; }

        public Librarian() { }

        public Librarian(string firstName, string lastName, int age, string passward, string id) 
            : base(firstName, lastName, age, passward, id) { }
    }
}
