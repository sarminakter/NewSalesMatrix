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
    public class RetailCustomerService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(RetailCustomer customer)
        {
            if (customer.Address != null && customer.Region != null && customer.MobileNo != null && customer.Gender != null)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                customer.CreatedDate = DateTime.UtcNow;
                customer.CreatedFrom = ipAddress;
                db.RetailCustomers.Add(customer);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Update(RetailCustomer customer)
        {
            if (customer.Address != null && customer.Region != null && customer.MobileNo != null && customer.Gender != null)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                customer.ModifiedDate = DateTime.UtcNow;
                customer.ModifiedFrom = ipAddress;

                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();


            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Delete(int id)
        {
            RetailCustomer customer = db.RetailCustomers.Find(id);
            db.RetailCustomers.Remove(customer);
            db.SaveChanges();
        }

        public List<RetailCustomer> GetAll()
        {
            List<RetailCustomer> customerList = db.RetailCustomers.ToList();
            return customerList;
        }

        public RetailCustomer GetById(int id)
        {
            return db.RetailCustomers.Find(id);
        }

    }
}
