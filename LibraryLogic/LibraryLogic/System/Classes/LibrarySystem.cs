using DAL;
using LibraryLogic.System.Classes;
using LibraryLogic.System.Interfaces;
using Services.Classes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace LibraryOOPAssignment
{
    public static class LibrarySystem
    {
        public static ILibraryCollectionManager _library;
        public static IUsersManager _userManager;
        public static IDiscountManager _discountManager;
        public static IReportsManager _reportsManager;

        public static void Load() // Loads all library data
        {
            using (var context = new LibraryContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            IContextFactory contextFactory = new ContextFactory();
            IReportGenerator reportGenerator = new ReportGenerator();
            _library = new LibraryCollectionManager("Modi'in library", contextFactory);
            _userManager = new UsersManager(contextFactory);
            _discountManager = new DiscountManager(contextFactory);
            _reportsManager = new ReportsManager(contextFactory, reportGenerator);
        }
    }
}