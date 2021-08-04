using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.Items
{
    public class BorrowingHistory
    {
        public virtual int Id { get; set; }

        public virtual int BorrowerId { get; set; }
        public virtual Person Borrower { get; set; }

        public virtual int ItemId { get; set; }
        public virtual AbstractItem Item { get; set; }

        public virtual IList<BorrowingHistoryReports> Reports { get; set; }

        public virtual DateTime BorrowingDate { get; set; }

        public virtual DateTime ReturningDate { get; set; }

        public BorrowingHistory() { }

        public BorrowingHistory(Person borrower, AbstractItem item)
        {
            Borrower = borrower;
            Item = item;
            BorrowingDate = DateTime.Now;
        }
    }
}
