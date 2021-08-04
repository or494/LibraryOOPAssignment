using LibraryLogic.System.Interfaces;
using LibraryOOPAssignment;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryLogic.System.Classes
{
    public class DiscountManager : IDiscountManager
    {
        private IContextFactory ContextFactory { get; set; }

        public DiscountManager(IContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public List<Discount> GetDiscounts()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void RemoveDiscount(Discount discount)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void SetDiscount(DiscountCategories category, float percent, string categoryItemName)
        {
            throw new NotImplementedException();
        }
    }
}
