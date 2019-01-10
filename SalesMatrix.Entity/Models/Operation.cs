using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
   public class Operation
    {
        
        public int Id { get; set; }
        public string OperationName { get; set; }
        public string Description { get; set; }       
        public bool IsGlobal { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedFrom { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedFrom { get; set; }
        public bool Status { get; set; }
        public int? ModuleId { get; set; }

        public virtual Module Module { get; set; }
        //public virtual List<Permission> Permissions { get; set; }
    }
}
