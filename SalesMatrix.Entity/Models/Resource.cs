using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
    public class Resource
    {
        
        public int Id { get; set; }

        public string ResourceType { get; set; }
        //[Index(IsUnique = true)]
        public string ResourceName { get; set; }
        public string Description { get; set; }
        public int? Parent { get; set; }
        public int Sequence { get; set; }        
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
        [ForeignKey("Parent")]
        public virtual Resource ParentResource { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        //public virtual List<Permission> Permissions { get; set; }
    }
}
