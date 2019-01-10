using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int? ParentItemId { get; set; }
        public int DisplaySequence { get; set; }
        public int RecorderLevel { get; set; }
        public bool IsActualItem { get; set; }
        public string Picture { get; set; }
        public bool Status { get; set; }

        //Audit Info
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }  
        public string CreatedFrom { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedFrom { get; set; }

        [ForeignKey("ParentItemId")]
        public Item ParentItem { get; set; }    
        public virtual List<Item> Items { get; set; }
        public virtual Discount Discount { get; set; }

    }
}
