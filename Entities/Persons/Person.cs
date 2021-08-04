using Entities.Items;
using LibraryLogic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibraryOOPAssignment
{
    [XmlInclude(typeof(Librarian))]
    [XmlInclude(typeof(Customer))]
    public abstract class Person
    {
        public virtual string Id { get;  set; }//private set
        public virtual string FirstName { get; set; }//private set
        public virtual string LastName { get;  set; }//private set
        public virtual int Age { get;  set; }//private set
        public virtual DateTime RegistrationDate { get;  set; }//private set
        public virtual string Passward { get;  set; }//private set
        public virtual ICollection<BorrowingHistory> BorrowingHistories { get; set; }
        public virtual ICollection<ItemCustomerViewed> ViewedItems { get; set; }

        public Person() { }

        public Person(string firstName, string lastName, int age, string passward,string id)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            RegistrationDate = DateTime.Now;
            Passward = passward;
            if (id.ToString().Length != 9)
                throw new ArgumentException("ID is invalid");
            else
                Id = id;
        }
    }
}
