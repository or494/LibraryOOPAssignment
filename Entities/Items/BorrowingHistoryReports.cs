using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.Items
{
    public class BorrowingHistoryReports
    {
        public virtual int Id { get; set; }

        public virtual int BorrowingHistoryId { get; set; }
        public virtual BorrowingHistory BorrowingHistory { get; set; }

        public virtual int ReportId { get; set; }
        public virtual Report Report { get; set; }
    }
}
