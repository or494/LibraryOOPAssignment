using LibraryOOPAssignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.System.Interfaces
{
    public interface IDiscountManager
    {
        void SetDiscount(DiscountCategories category, float percent, string categoryItemName);

        void RemoveDiscount(Discount discount);

        List<Discount> GetDiscounts();
    }
}
