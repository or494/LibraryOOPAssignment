using LibraryLogic.Items;
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
    public class ReportsManager : IReportsManager
    {
        private IContextFactory ContextFactory { get; set; }
        private IReportGenerator ReportGenerator { get; set; }

        public ReportsManager(IContextFactory contextFactory, IReportGenerator reportGenerator)
        {
            ContextFactory = contextFactory;
            ReportGenerator = reportGenerator;
        }

        public Report GenerateReport()
        {
            Report report = ReportGenerator.Generate(LibrarySystem._library.GetCurrentBorrowedItems(), LibrarySystem._library.GetUnborowedItems());
            using(var context = ContextFactory.CreateLibraryContext())
            {
                Librarian librarian = context.Users.FirstOrDefault(user => user.Id == LibrarySystem._userManager.GetLoggedUser().Id) as Librarian;
                if (librarian == null) throw new ArgumentException();

                librarian.ReportsCreated.Add(report);
                context.SaveChanges();
                return report;
            }
        }

        public Report GetReport(DateTime dateTime)
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                Report report = context.Reports.FirstOrDefault(x => x.Date.ToString() == dateTime.ToString());
                if (report == null) throw new ArgumentException();

                return report;
            }
        }

        public IList<BorrowingHistoryReports> GetReportBorrowingHistory(Report report)
        {
            using (var context = ContextFactory.CreateLibraryContext())
            {
                Report reportInstance = context.Reports.FirstOrDefault(x => x.Id == report.Id);
                if (report == null) throw new ArgumentException();

                return reportInstance.BorrowingHistories;
            }
        }

        public List<Report> GetReports()
        {
            using(var context = ContextFactory.CreateLibraryContext())
            {
                return context.Reports.ToList();
            }
        }
    }
}