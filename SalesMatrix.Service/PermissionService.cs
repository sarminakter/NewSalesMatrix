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
    public class PermissionService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(List<Permission> permission)
        {
            if (permission != null)
            {
                db.Permissions.AddRange(permission);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void CreateAndUpdate(List<Permission> permission)
        {
            if (permission != null)
            {
                foreach (var item in permission)
                {
                    Permission per = new Permission();
                    per = db.Permissions.Find(item.Id);
                    per.IsCreate = item.IsCreate;
                    per.IsRead = item.IsRead;
                    per.IsEdit = item.IsEdit;
                    per.IsDelete = item.IsDelete;
                    per.IsPrint = item.IsPrint;
                    per.IsExclusive = item.IsExclusive;
                    db.Entry(per).State = EntityState.Modified;                    
                }
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Update(Permission permission)
        {
            if (permission.PermissionType != null)
            {

                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

               permission.ModifiedDate = DateTime.UtcNow;
                permission.ModifiedFrom = ipAddress;

                db.Entry(permission).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public List<Permission> GetAll()
        {
            return db.Permissions.ToList();
        }
        //public List<Permission> GetAllByModuleId(int moduleId)
        //{
        //    List<Permission> newPermissionList = new List<Permission>();            
        //    var permissionList = db.Permissions.ToList();

        //    OperationService opService = new OperationService();
        //    List<Operation> operationList = opService.GetAllByModuelId(moduleId);
        //    List<int?> operationIdArray=new List<int?>();
        //    foreach (var item in operationList)
        //    {
        //        operationIdArray.Add(item.Id);
        //        var exist = permissionList.FirstOrDefault(p => p.OperationId == item.Id);
        //        if (exist==null)
        //        {
        //            Permission permission = new Permission();
        //            permission.OperationId = item.Id;
        //            permission.Name = item.OperationName;
        //            newPermissionList.Add(permission);
        //        }
        //    }

        //    ResourceService resService = new ResourceService();
        //    List<Resource> resourceList = resService.GetAllByModuelId(moduleId);
        //    List<int?> resourceIdArray = new List<int?>();
        //    foreach (var item in resourceList)
        //    {
        //        resourceIdArray.Add(item.Id);
        //        var exist = permissionList.FirstOrDefault(p => p.ResourceId == item.Id);
        //        if (exist == null)
        //        {
        //            Permission permission = new Permission();
        //            permission.ResourceId = item.Id;
        //            permission.Name = item.ResourceName;
        //            newPermissionList.Add(permission);
        //        }
        //    }
        //    try
        //    {
        //        Create(newPermissionList);
        //        return db.Permissions.Where(p => operationIdArray.Contains(p.OperationId) || resourceIdArray.Contains(p.ResourceId)).ToList();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }            
            
        //}

        public List<Permission> GetAllByModuleRoleOrUserId(int moduleId,int roleId,int? userId)
        {
            List<Permission> newPermissionList = new List<Permission>();
            var permissionList = db.Permissions.ToList();

            OperationService opService = new OperationService();
            List<Operation> operationList = opService.GetAllByModuelId(moduleId);
            List<int?> operationIdArray = new List<int?>();
            foreach (var item in operationList)
            {
                operationIdArray.Add(item.Id);
                var exist = permissionList.FirstOrDefault(p => p.OperationId == item.Id && p.ModuleId == moduleId && p.RoleId==roleId && p.UserId==userId);
                if (exist == null)
                {
                    Permission permission = new Permission();
                    permission.OperationId = item.Id;
                    permission.Name = item.OperationName;
                    permission.PermissionType = "Operation";
                    permission.ModuleId = moduleId;
                    permission.RoleId = roleId;
                    permission.UserId = userId;
                    newPermissionList.Add(permission);
                }
            }

            ResourceService resService = new ResourceService();
            List<Resource> resourceList = resService.GetAllByModuelId(moduleId);
            List<int?> resourceIdArray = new List<int?>();
            foreach (var item in resourceList)
            {
                resourceIdArray.Add(item.Id);
                var exist = permissionList.FirstOrDefault(p => p.ResourceId == item.Id && p.ModuleId == moduleId && p.RoleId == roleId && p.UserId == userId);
                if (exist == null)
                {
                    Permission permission = new Permission();
                    permission.ResourceId = item.Id;
                    permission.Name = item.ResourceName;
                    permission.PermissionType = "Resource";
                    permission.ModuleId = moduleId;
                    permission.RoleId = roleId;
                    permission.UserId = userId;
                    newPermissionList.Add(permission);
                }
            }
            try
            {
                Create(newPermissionList);
                return db.Permissions.Where(p =>p.ModuleId == moduleId && p.RoleId == roleId && p.UserId == userId).ToList();
                //return db.Permissions.Where(p => operationIdArray.Contains(p.OperationId) || resourceIdArray.Contains(p.ResourceId)).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Permission GetById(int id)
        {
            return db.Permissions.Find(id);
        }
    }
}
