using LibraryLogic.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class Report
    {
        public virtual int Id { get; set; }

        public virtual string LibrarianId { get; set; }

        public virtual Librarian Librarian { get; set; }
         
        public virtual int ExistBooks { get; set; }

        public virtual int ExistJournals { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual IList<BorrowingHistoryReports> BorrowingHistories { get; set; }

        public Report() { }
    }
}
