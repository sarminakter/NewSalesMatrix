using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
    public class Pricing
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double Unit { get; set; }
        public double MRP { get; set; }
        public double? PurchasePrice { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedFrom { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedFrom { get; set; }

        public virtual Item Item { get; set; }

    }
}
