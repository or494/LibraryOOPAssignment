using DAL;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classes
{
    public class ContextFactory : IContextFactory
    {
        public LibraryContext CreateLibraryContext()
        {
            return new LibraryContext();
        }
    }
}
