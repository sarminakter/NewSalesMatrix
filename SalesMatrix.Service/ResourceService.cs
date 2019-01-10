using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Service
{
    public class ResourceService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(Resource resource)
        {
            if (resource.ResourceName != null && resource.ResourceName != "" && resource.ResourceType != null && resource.ResourceType != "" && resource.Description != null && resource.Description != "" && resource.Sequence != 0)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                resource.CreatedDate = DateTime.UtcNow;
                resource.CreatedFrom = ipAddress;
                db.Resources.Add(resource);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Update(Resource resource)
        {
            //Resource updateResource = GetById(resource.Id);
            if (resource.ResourceName != null && resource.ResourceName != "" && resource.ResourceType != null && resource.ResourceType != "" && resource.Description != null && resource.Description != "" && resource.Sequence != 0)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                resource.ParentResource = null;
                resource.Module = null;

                //updateResource.Id = resource.Id;
                //updateResource.ModuleId = resource.ModuleId;
                //updateResource.ResourceType = resource.ResourceType;
                //updateResource.ResourceName = resource.ResourceName;
                //updateResource.Description = resource.Description;
                //updateResource.Parent = resource.Parent;
                //updateResource.Sequence = resource.Sequence;
                //updateResource.IsGlobal = resource.IsGlobal;
                //updateResource.CreatedBy = resource.CreatedBy;
                //updateResource.CreatedDate = resource.CreatedDate;
                //updateResource.CreatedFrom = resource.CreatedFrom;
                //updateResource.Status = resource.Status;
                //updateResource.ModuleId = resource.ModuleId;
                //updateResource.ModifiedBy = null;
                //updateResource.ModifiedDate = DateTime.UtcNow;
                //updateResource.ModifiedFrom = ipAddress;
                resource.ModifiedDate = DateTime.UtcNow;
                resource.ModifiedFrom = ipAddress;

                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public List<Resource> GetAll()
        {
            return db.Resources.ToList();
            //var list = (from r in db.Resources
            //            select new
            //            {
            //                r.Id,
            //                r.ResourceType,
            //                r.ResourceName,
            //                r.Description,
            //                r.Parent,
            //                r.Sequence,
            //                Modules = (from m in db.Modules
            //                           where m.Id == r.ModuleId
            //                           select new
            //                           {
            //                               m.Id,
            //                               m.ModuleName,
            //                               m.Description,
            //                               m.Status,
            //                               m.CreatedBy,
            //                               m.CreatedDate,
            //                               m.CreatedFrom,
            //                               m.ModifiedBy,
            //                               m.ModifiedDate,
            //                               m.ModifiedFrom
            //                           }).FirstOrDefault(),
            //                r.IsGlobal,
            //                r.CreatedBy,
            //                r.CreatedDate,
            //                r.CreatedFrom,
            //                r.ModifiedBy,
            //                r.ModifiedDate,
            //                r.ModifiedFrom,
            //                r.Status
            //            }).ToList();

            //return list.ToList<dynamic>();
        }

        public List<Resource> GetAllByModuelId(int moduleId)
        {
            return db.Resources.Where(r=>r.ModuleId==moduleId).ToList();            
        }
        
        public Resource GetById(int id)
        {
            return db.Resources.Find(id);
        }
    }
}
