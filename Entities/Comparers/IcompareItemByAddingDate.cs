using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class IcompareItemByAddingDate : IComparer<AbstractItem>
    {
        public int Compare(AbstractItem x, AbstractItem y)
        {
            if (x.AddedAtDate.CompareTo(y.AddedAtDate) < 0)
                return 1;
            else if (x.AddedAtDate.CompareTo(y.AddedAtDate) > 0)
                return -1;
            else
            {
                return x.Name.CompareTo(y.Name);
            }
        }
    }
}
