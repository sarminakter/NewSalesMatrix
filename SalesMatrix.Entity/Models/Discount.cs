using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
    public class Discount
    {
        public int? Id { get; set; }
        public string DiscountTitle { get; set; }
        public string Description { get; set; }
        public int? ItemId { get; set; }
        public int? CategoryId { get; set; }
        public double DiscountRate { get; set; }
        public double DiscountAmount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public bool IsContinue { get; set; }

        //Audit Info
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedFrom { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedFrom { get; set; }

        public virtual List<Item> Items { get; set; }   
    }
}
