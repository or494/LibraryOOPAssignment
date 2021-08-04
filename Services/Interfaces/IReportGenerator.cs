using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReportGenerator
    {
        Report Generate(List<AbstractItem> BorrowedItems, List<AbstractItem> UnborrowedItems);
    }
}
