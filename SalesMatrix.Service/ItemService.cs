using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SalesMatrix.Service
{
    public class ItemService
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();

        public void Create(Item item)
        {
            if (item.ItemName != null && item.ParentItemId != 0)
            {
                if (db.Items.FirstOrDefault(i => i.ItemName == item.ItemName) == null)
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                    var ipAddress = ipList[0].ToString();

                    item.CreatedDate = DateTime.UtcNow;
                    item.CreatedFrom = ipAddress;
                    db.Items.Add(item);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("This item already exist");
                }
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Update(Item item)
        {
            if (item.ItemName != null && item.ParentItemId != 0)
            {
                if (db.Items.FirstOrDefault(i => i.ItemName == item.ItemName && i.Id != item.Id) == null)
                {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                    var ipAddress = ipList[0].ToString();

                    item.ModifiedDate = DateTime.UtcNow;
                    item.ModifiedFrom = ipAddress;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("This item already exist");
                }
            }
            else
            {
                throw new Exception("Required field can't be empty");
            }
        }

        public void Delete(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
        }

        public List<Item> GetAll()
        {
            List<Item> itemList = db.Items.ToList();
            return itemList;
        }

        public List<Item> GetAllActiveItem()
        {
            List<Item> itemList = db.Items.Where(i => i.Status == true).ToList();
            return itemList;
        }

        public List<Item> GetAllInactiveItem()
        {
            List<Item> itemList = db.Items.Where(i => i.Status == false).ToList();
            return itemList;
        }

        public List<Item> GetAllActualItem()
        {
            List<Item> itemList = db.Items.Where(i => i.IsActualItem == true).ToList();
            return itemList;
        }

        public List<Item> GetAllNotActualItem()
        {
            List<Item> itemList = db.Items.Where(i => i.IsActualItem == false).ToList();
            return itemList;
        }

        public Item GetById(int id)
        {
            return db.Items.Find(id);
        }

        public bool NameExists(string name)
        {
            return db.Items.Count(i => i.ItemName == name) > 0;
        }

        public bool NameExistsForEdit(string name, int id)
        {
            return db.Items.Count(i => i.ItemName == name && i.Id != id) > 0;
        }
    }
}
