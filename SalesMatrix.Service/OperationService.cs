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
    public class OperationService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(Operation operation)
        {
            if (operation.OperationName != null && operation.Description != "")
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                operation.CreatedDate = DateTime.UtcNow;
                operation.CreatedFrom = ipAddress;
                db.Operations.Add(operation);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Update(Operation operation)
        {
            if (operation.OperationName != null && operation.Description != "")
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                operation.Module = null;

                operation.ModifiedDate = DateTime.UtcNow;
                operation.ModifiedFrom = ipAddress;

                db.Entry(operation).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public List<Operation> GetAll()
        {
            return db.Operations.ToList();            
        }

        public List<Operation> GetAllByModuelId(int moduleId)
        {
            return db.Operations.Where(o=>o.ModuleId==moduleId).ToList();
        }

        public Operation GetById(int id)
        {
            return db.Operations.Find(id);
        }
    }
}
