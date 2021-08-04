using LibraryOOPAssignment;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classes
{
    public class ReportGenerator : IReportGenerator
    {
        public Report Generate(List<AbstractItem> BorrowedItems, List<AbstractItem> UnborrowedItems)
        {
            List<AbstractItem> allitems = new List<AbstractItem>(BorrowedItems.Count + UnborrowedItems.Count);
            allitems.AddRange(BorrowedItems);
            allitems.AddRange(UnborrowedItems);

            Report report = new Report();
            report.ExistBooks = GetBooksCount(allitems);
            report.ExistJournals = GetJournalsCount(allitems);      
            report.Date = DateTime.Now;
            return report;
        }

        private int GetBooksCount(List<AbstractItem> items)
        {
            int ret = 0;
            foreach(var item in items)
            {
                if (item as Book != null) ret++;
            }
            return ret;
        }

        private int GetJournalsCount(List<AbstractItem> items)
        {
            int ret = 0;
            foreach (var item in items)
            {
                if (item as Journal != null) ret++;
            }
            return ret;
        }
    }
}