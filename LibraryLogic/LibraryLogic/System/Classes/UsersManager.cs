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
    public class UsersManager : IUsersManager
    {
        private Person LoggedUser { get; set; }
        private IContextFactory ContextFactory { get; set; }

        public UsersManager(IContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public Person GetLoggedUser()
        {
            return LoggedUser;
        }

        public Person GetPerson(string id)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                return context.Users.FirstOrDefault(user => user.Id == id);
            }
        }

        public bool LogIn(string id, string passward)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                Person person = context.Users.FirstOrDefault(user => user.Id == id && user.Passward == passward);
                if (person == null) return false;
                else
                {
                    SetLoggedUser(person);
                    return true;
                }
            }
        }

        public void LogOut()
        {
            SetLoggedUser(null);
        }

        public bool Register(Person person)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                Person registeringUser = context.Users.FirstOrDefault(user => person.Id == user.Id);
                if (registeringUser != null) return false;

                context.Users.Add(person);
                context.SaveChanges();
                return true;
            }
        }

        public void SetLoggedUser(Person person)
        {
            LoggedUser = person;
        }
    }
}
