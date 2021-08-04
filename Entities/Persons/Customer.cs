using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class Customer : Person
    {
        public int CanBorrow { get; set; }//private set 

        public Customer() { }

        public Customer(string firstName, string lastName, int age, string passward, string id)
            : base(firstName,lastName,age,passward,id)
        {
            CanBorrow = 3;
        }
    }
}
