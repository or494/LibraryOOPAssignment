using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class IcompareItemByPublisher : IComparer<AbstractItem>
    {
        public int Compare(AbstractItem x, AbstractItem y)
        {
            return x.Publisher.CompareTo(y.Publisher);
        }
    }
}
