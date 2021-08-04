using Entities.Items;
using LibraryLogic.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibraryOOPAssignment
{
    [XmlInclude(typeof(Book))]
    [XmlInclude(typeof(Journal))]
    public abstract class AbstractItem
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }//protected set
        public virtual CategoryName Category { get; set; }//protected set
        public virtual string Publisher { get; set; }//protected set
        public virtual float Price { get; set; }//protected set
        public virtual int? Edition { get; set; }//protected set
        public virtual DateTime PrintYear { get; set; }//protected set
        public virtual bool IsBorrowed { get; set; }//protected set
        public virtual DateTime AddedAtDate { get; set; }//private set
        public virtual string ImgName { get; set; }//private set
        public virtual Guid ISBN { get; set; }//private set
        public virtual Discount Discount { get; set; }
        public virtual IList<BorrowingHistory> BorrowingHistories { get; set; }
        public virtual ICollection<ItemCustomerViewed> PersonViewed { get; set; }

        public virtual int AddedById { get; set; }
        public virtual Librarian AddedBy { get; set; }

        public AbstractItem() { }

        public AbstractItem(string name, CategoryName category, string publisher, float price, int? edition, DateTime printYear,string imgName, Librarian librarian)
        {
            Name = name;
            Category = category;
            Publisher = publisher;
            Price = price;
            Edition = edition;
            PrintYear = printYear;
            IsBorrowed = false;
            BorrowingHistories = new List<BorrowingHistory>();
            ImgName = imgName;
            ISBN = Guid.NewGuid();
            AddedBy = librarian;
        }

        [XmlIgnore]
        [NotMapped]
        public Discount CurrentDiscount
        {
            get { return Discount; }
            set
            {
                Discount tmp = Discount;
                this.Discount = value;
                //OnDiscountChanged(tmp, Discount);
            }
        }
    }
}
