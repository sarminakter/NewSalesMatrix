using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesMatrix.Entity.Models
{
   public class _SalesMatrixDB:DbContext
    {
        public _SalesMatrixDB():base("SalesMatrixDB")
        {

        }
        //public DbSet<RoleTable> RoleTables { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Permission> Permissions { get; set; }       
        public DbSet<Item> Items { get; set; }       
        public DbSet<RetailCustomer> RetailCustomers { get; set; }          
        public DbSet<Pricing> Pricings { get; set; }              
        public DbSet<Discount> Discounts { get; set; }              

    }
}
