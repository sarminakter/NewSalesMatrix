using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Service.Model
{
    public class ModuleService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(Module module)
        {
            if (module.ModuleName != null && module.ModuleName != "")
            {
                if (db.Modules.FirstOrDefault(m=>m.ModuleName == module.ModuleName) == null)
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                    var ipAddress = ipList[0].ToString();

                    module.CreatedDate = DateTime.UtcNow;
                    module.CreatedFrom = ipAddress;
                    db.Modules.Add(module);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("This module already exist");                    
                }
            }
            else
            {
                throw new Exception("Module name can't be empty");
            }
        }

        public void Update(Module module)
        {
            if (module.ModuleName != null && module.ModuleName != "")
            {
                if (db.Modules.FirstOrDefault(m => m.ModuleName == module.ModuleName) == null)
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                    var ipAddress = ipList[0].ToString();

                    module.ModifiedDate = DateTime.UtcNow;
                    module.ModifiedFrom = ipAddress;
                    db.Entry(module).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("This module already exist");
                }
            }
            else
            {
                throw new Exception("Module name can't be empty");
            }
        }

        public List<Module> GetAll()
        {
            return db.Modules.ToList();
        }

        public List<Module> GetAllActiveModule()
        {
            return db.Modules.Where(m=>m.Status==true).ToList();
        }

        public List<Module> GetAllInactiveModule()
        {
            return db.Modules.Where(m => m.Status == false).ToList();
        }

        public Module GetById(int id)
        {
            return db.Modules.Find(id);
        }

        public bool ModuleExists(string name)
        {
            return db.Modules.Count(m => m.ModuleName == name) > 0;
        }

        public bool ModuleExistsForEdit(string name, int id)
        {
            return db.Modules.Count(m => m.ModuleName == name && m.Id != id) > 0;
        }
    }    
}
