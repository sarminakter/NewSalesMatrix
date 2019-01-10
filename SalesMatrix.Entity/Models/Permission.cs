using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
    public class Permission
    {        
        public int Id { get; set; }
        public string PermissionType { get; set; }
        public string Name { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPrint { get; set; }
        public bool IsExclusive { get; set; }     
        
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public int? ResourceId { get; set; }
        public int? OperationId { get; set; }
        public int? ModuleId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedFrom { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedFrom { get; set; }
        
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
        public virtual Resource Resource { get; set; }
        public virtual Operation Operation { get; set; } 

    }
}
