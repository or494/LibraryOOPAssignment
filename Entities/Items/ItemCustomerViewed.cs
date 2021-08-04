using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Items
{
    public class ItemCustomerViewed
    {
        public int Id { get; set; }

        public virtual int BorrowerId { get; set; }
        public virtual Person Borrower { get; set; }

        public virtual int ItemId { get; set; }
        public virtual AbstractItem Item { get; set; }

        public ItemCustomerViewed() { }

        public ItemCustomerViewed(Person borrower, AbstractItem item)
        {
            Borrower = borrower;
            Item = item;
        }
    }
}
