using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Service
{
    public class PricingService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(Pricing price)
        {
            if (price.ProductId != null)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();

                price.CreatedDate = DateTime.UtcNow;
                price.CreatedFrom = ipAddress;
                db.Pricings.Add(price);
                db.SaveChanges();
            }
            else
            {
                throw new Exception("This module already exist");
            }
        }

        public List<Pricing> GetAll()
        {
            return db.Pricings.ToList();
        }
    }
}
