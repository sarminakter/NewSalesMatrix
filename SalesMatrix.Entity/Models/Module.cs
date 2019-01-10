using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Index(IsUnique = true)]
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        //Audit Info
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedFrom { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedFrom { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
