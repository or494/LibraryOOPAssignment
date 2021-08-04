using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class IcompareItemLowerByPrice : IComparer<AbstractItem>
    {
        public int Compare(AbstractItem x, AbstractItem y)
        {
             return x.Price.CompareTo(y.Price);
        }
    }
}
