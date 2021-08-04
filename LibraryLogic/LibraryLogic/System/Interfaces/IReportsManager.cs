using LibraryLogic.Items;
using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.System.Interfaces
{
    public interface IReportsManager
    {
        Report GenerateReport();

        Report GetReport(DateTime dateTime);

        IList<BorrowingHistoryReports> GetReportBorrowingHistory(Report report);

        List<Report> GetReports();
    }
}
