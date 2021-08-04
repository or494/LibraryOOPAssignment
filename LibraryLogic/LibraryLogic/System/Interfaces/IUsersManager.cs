using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.System.Interfaces
{
    public interface IUsersManager
    {
        bool LogIn(string id, string passward);

        bool Register(Person person);

        Person GetPerson(string id);

        void LogOut();

        Person GetLoggedUser();

        void SetLoggedUser(Person person);
    }
}
