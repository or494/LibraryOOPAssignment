using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class IcompareItemByName : IComparer<AbstractItem>
    {
        int IComparer<AbstractItem>.Compare(AbstractItem x, AbstractItem y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
