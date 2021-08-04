using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryOOPAssignment
{
    public class Discount : ICloneable, IComparable
    {
        public virtual int Id { get; set; }
        public virtual DiscountCategories Category { get; set; }
        public virtual float DiscountPercent { get; set; }
        public virtual string DiscountReasonName { get; set; }

        public Discount() { }

        public Discount(DiscountCategories category,float percent,string categoryItemName)
        {
            Category = category;
            DiscountPercent = percent;
            DiscountReasonName = categoryItemName;
        }

        public object Clone()
        {
            Discount tmp = new Discount(Category,DiscountPercent,DiscountReasonName);
            return tmp;
        }

        public int CompareTo(object obj)
        {
            Discount tmp = obj as Discount;
            if(tmp != null)
            {
                if (tmp.DiscountPercent == this.DiscountPercent && tmp.Category == this.Category &&
                    tmp.DiscountReasonName == this.DiscountReasonName) return 0;
            }
            return 1;
        }
    }
}
